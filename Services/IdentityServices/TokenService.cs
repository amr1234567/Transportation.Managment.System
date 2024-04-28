using ECommerce.Core.Interfaces.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Dto.Identity;
using Core.Identity;
using Core.Helpers.Classes;

namespace ECommerce.InfaStructure.Services
{
    public class TokenService(IOptions<JwtHelper> config) : ITokenService
    {
        private SymmetricSecurityKey _key = new SymmetricSecurityKey(new byte[10]);
        private readonly JwtHelper _config = config.Value;

        public Task<TokenModel> CreateToken(User user, List<string> roles, List<Claim>? InternalClaims = null)
        {
            if (user is null)
                throw new ArgumentNullException("User Can't be null");

            if (roles is null || roles.Count == 0)
                throw new ArgumentNullException("Roles Can't be null");

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.Name),
                new("Id", user.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (InternalClaims is not null)
                _ = claims.Union(InternalClaims);


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
