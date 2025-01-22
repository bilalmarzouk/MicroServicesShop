using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shop.Services.AuthApi.Model;
using Shop.Services.AuthApi.Model.Dto;
using Shop.Services.AuthApi.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop.Services.AuthApi.Service
{
    public class JWTTokenGenrator : IJWTTokenGenrator
    {
        private JwtOptions _jwtOptions;

        public JWTTokenGenrator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenrateToken(ApplicationUsers applicationuser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationuser.Email),
                new Claim(JwtRegisteredClaimNames.Sub,applicationuser.Id),
                new Claim(JwtRegisteredClaimNames.Name,applicationuser.UserName),
            };
            var tokendescripter = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokendescripter);
            return tokenHandler.WriteToken(token);
        }
    }
}
