using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Moneta.Core.Jwt;

namespace Moneta.Frontend.API
{
    public class AdminUserTransformation : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            Claim id = principal.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.UserName);

            if(id.Value == "jellebens@outlook.com") {
                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            }

            return principal;
        }

        
    }
}
