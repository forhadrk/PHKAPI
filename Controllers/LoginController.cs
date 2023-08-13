using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PHKAPI.JwtTokens;
using PHKAPI.Models;
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
        const string _spName = "SP_GET_LOGIN";

        public LoginController(IJWTManagerRepository jWTManager, IConfiguration iconfiguration, DatabaseContext dbContext, IDapper dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
            _iconfiguration = iconfiguration;
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(LoginUserDBModel loginModel)
        {
            // Authenticate user based on credentials (customize this part based on your database schema)
            List<LoginUserDBModel> _loginUser = await GetUserFromDatabase(loginModel.UserName, loginModel.Password);
            var user = _loginUser.SingleOrDefault();
            if (user == null)
            {
                return Unauthorized();
            }

            // Create claims for the JWT token
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

            var JWTtoken = new LoginUserDBModel { 
                LoginUserID = user.LoginUserID,
                UserName = user.UserName.ToString(), 
                Name = user.Name.ToString(), 
                Email = user.Email.ToString(), 
                Token = new JwtSecurityTokenHandler().WriteToken(token) 
            };

            return Ok(JWTtoken);
        }

        // Replace this method with your actual database query logic
        private async Task<List<LoginUserDBModel>> GetUserFromDatabase(string userName, string password)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("QryOption", 1, DbType.Int32);
            dbparams.Add("UserName", userName, DbType.String);
            dbparams.Add("Password", password, DbType.String);
            return await Task.FromResult(_dapper.GetAll<LoginUserDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        }

        //[HttpPost("LoginUser")]
        //public async Task<List<LoginUserDBModel>> LoginUser(LoginUserDBModel _dbModel)
        //{
        //    var dbparams = new DynamicParameters();
        //    dbparams.Add("QryOption", 1, DbType.Int32);
        //    dbparams.Add("UserName", _dbModel.UserName, DbType.String);
        //    dbparams.Add("Password", _dbModel.Password, DbType.String);
        //    return await Task.FromResult(_dapper.GetAll<LoginUserDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure));
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("LoginUser")]
        //public IActionResult LoginUser(LoginUserDBModel _dbModel)
        //{

        //    var dbparams = new DynamicParameters();
        //    dbparams.Add("QryOption", 1, DbType.Int32);
        //    dbparams.Add("UserName", _dbModel.UserName, DbType.String);
        //    dbparams.Add("Password", _dbModel.Password, DbType.String);
        //    List<LoginUserDBModel> _obj = _dapper.GetAll<LoginUserDBModel>(_spName, dbparams, commandType: CommandType.StoredProcedure);


        //    var claims = new[] {
        //            new Claim(JwtRegisteredClaimNames.Sub, _iconfiguration["Jwt:Subject"]),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
        //            new Claim("LoginID", _obj[0].LoginUserID.ToString()),
        //            new Claim("UserName", _obj[0].UserName.ToString()),
        //            new Claim("Name", _obj[0].Name.ToString()),
        //            new Claim("Email", _obj[0].Email.ToString())
        //           };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["Jwt:Key"]));

        //    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        _iconfiguration["Jwt:Issuer"],
        //        _iconfiguration["Jwt:Audience"],
        //        claims,
        //        expires: DateTime.Now.AddMinutes(20),
        //        signingCredentials: signIn
        //     );

        //    var JWTtoken = new Tokens { Token = new JwtSecurityTokenHandler().WriteToken(token) };

        //    if (JWTtoken == null)
        //    {
        //        return Unauthorized();
        //    }

        //    return  Ok(JWTtoken);
        //}
    }
}
