using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DowntimeAlerter.Domain.Settings;
using DowntimeAlerter.Infrastructure.Helper.Contract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DowntimeAlerter.Infrastructure.Helper
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JWTSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> Generate(Claim[] claims)
        {
            claims.Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}