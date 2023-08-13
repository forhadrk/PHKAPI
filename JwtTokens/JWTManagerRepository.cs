using Microsoft.IdentityModel.Tokens;
using PHKAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PHKAPI.JwtTokens
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        Dictionary<string, string> UsersRecords = new Dictionary<string, string>
    {
        { "user1","password1"},
        { "user2","password2"},
        { "user3","password3"},
    };

        private readonly IConfiguration iconfiguration;
        public JWTManagerRepository(IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
        }
        public Tokens Authenticate(LoginUserDBModel users)
        {
            if (!UsersRecords.Any(x => x.Key == users.Name && x.Value == users.Password))
            {
                return null;
            }

            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, iconfiguration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    //new Claim("UserCode", user.UserCode.ToString()),
                    //new Claim("UserName", user.UserName),
                    //new Claim("Email", user.Email),
                    //new Claim("Name", user.Name)
                   };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconfiguration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                iconfiguration["Jwt:Issuer"],
                iconfiguration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: signIn
             );

            return new Tokens { Token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}
