using Core.Dto.Identity;
using Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<TokenModel> CreateToken(User user, List<string> roles, List<Claim>? Internalclaims = null);
    }
}
