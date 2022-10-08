using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ServiceLayer.Utilities.JWT
{
    public class JwtValidator
    {
        private readonly IConfiguration _configuration;


        public JwtValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = _configuration["Token:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Token:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]))
                }, out validatedToken);
            }
            catch
            {
                throw new Exception("Token validation failed.");
            }

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken;
        }
    }
}
