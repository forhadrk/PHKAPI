using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PHKAPI.JwtTokens;
using PHKAPI.Models;
using PHKAPI.Services;
using PMS.API.DBContext;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PHKAPI.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJWTManagerRepository _jWTManager;
        private readonly IConfiguration _iconfiguration;
        private readonly IDapper _dapper;
        public readonly DatabaseContext _dbContext;
        private readonly EmailService _emailService;
        const string _spName = "SP_GET_LOGIN";

        public LoginController(IJWTManagerRepository jWTManager, IConfiguration iconfiguration, DatabaseContext dbContext, IDapper dapper, EmailService emailService)
        {
            _dbContext = dbContext;
            _dapper = dapper;
            _iconfiguration = iconfiguration;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(LoginUserDBModel loginModel)
        {
            try
            {

                string otpCode = GenerateRandomOTP();
                List<LoginUserDBModel> _loginUser = await GetUserFromDatabase(loginModel.UserName, loginModel.Password, otpCode);
                var user = _loginUser.SingleOrDefault();
                if (user == null)
                {
                    return Unauthorized();
                }

                //EmailDBModel emailModel = new EmailDBModel();
                //emailModel.To = user.Email.ToString();
                //_emailService.SendLoginEmail(emailModel, otpCode);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _iconfiguration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim("LoginUserID", user.LoginUserID.ToString()),
                    new Claim("UserName", user.UserName.ToString()),
                    new Claim("Name", user.Name.ToString()),
                    new Claim("Email", user.Email.ToString())
                };

                // Generate JWT token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _iconfiguration["Jwt:Issuer"],
                    _iconfiguration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    signingCredentials: signIn
                );

                var JWTtoken = new LoginUserDBModel
                {
                    LoginUserID = user.LoginUserID,
                    UserName = user.UserName.ToString(),
                    Name = user.Name.ToString(),
                    Email = user.Email.ToString(),
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };

                return Ok(JWTtoken);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        static string GenerateRandomOTP()
        {
            // Replace this with your OTP generation logic (e.g., generating a random 6-digit code)
            Random random = new Random();
            int otp = random.Next(1000, 10000);
            return otp.ToString();
        }
        // Replace this method with your actual database query logic
        private async Task<List<LoginUserDBModel>> GetUserFromDatabase(string userName, string password, string otpCode)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 1, DbType.Int32);
            dbparams.Add("UserName", userName, DbType.String);
            dbparams.Add("Password", password, DbType.String);
            dbparams.Add("OTP", otpCode, DbType.String);
            return await Task.FromResult(_dapper.GetAll<LoginUserDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }
    }
}
