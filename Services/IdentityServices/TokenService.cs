using ECommerce.Core.Interfaces.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Dto.Identity;
using Core.Identity;
using Core.Helpers;

namespace ECommerce.InfaStructure.Services
{
    public class TokenService : ITokenService
    {
        private SymmetricSecurityKey _key;
        private readonly JwtHelper _config;

        public TokenService(IOptions<JwtHelper> config)
        {
            _key = new SymmetricSecurityKey(new byte[10]);
            _config = config.Value;
        }

        public Task<TokenModel> CreateToken(User user, List<string> roles, List<Claim>? InternalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (InternalClaims is not null)
                claims.Union(InternalClaims);


            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));

            var ExpierdOn = DateTime.UtcNow.AddMinutes(_config.expirePeriodInMinuts);

            var SecurityToken = new JwtSecurityToken(
                issuer: _config.issuer,
                signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                expires: ExpierdOn
                );

            return Task.FromResult(new TokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken),
                TokenExpiration = ExpierdOn
            });
        }
    }
}
