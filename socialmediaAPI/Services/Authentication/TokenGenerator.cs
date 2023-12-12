using Microsoft.IdentityModel.Tokens;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace socialmediaAPI.Services.Authentication
{
    public class TokenGenerator
    {
        private readonly TokenConfigs _tokenConfigs;

        public TokenGenerator(TokenConfigs tokenConfigs)
        {
            _tokenConfigs = tokenConfigs;
        }
        public string GenerateAccessToken(User user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _tokenConfigs.AccessTokenSecret));
            SigningCredentials credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.ID),
                new Claim(ClaimTypes.Email,user.AuthenticationInfo.Email)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _tokenConfigs.Issuer,
                _tokenConfigs.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_tokenConfigs.AccessTokenExpirationMinutes),
                credential
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
