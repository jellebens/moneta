using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Services
{
    public interface IJwtTokenBuilder
    {
        string Build(ClaimsPrincipal principal);
    }

    public class JwtTokenBuilder : IJwtTokenBuilder
    {
        private readonly IConfiguration _Configuration;
        private readonly ILogger<JwtTokenBuilder> _Logger;

        public JwtTokenBuilder(IConfiguration configuration, ILogger<JwtTokenBuilder> logger)
        {
            this._Configuration = configuration;
            this._Logger = logger;
        }
        public string Build(ClaimsPrincipal principal)
        {
            string name = principal.Claims.FirstOrDefault(c => c.Type.Equals("name", StringComparison.CurrentCultureIgnoreCase)).Value;
            string id = principal.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username", StringComparison.CurrentCultureIgnoreCase)).Value; ;

            var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT_SECRET"]));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Name, name)
                }),
                NotBefore = DateTime.UtcNow.AddMinutes(-15),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = "https://login.microsoftonline.com/common",
                Audience = _Configuration.GetValue<string>("CLIENT_ID"),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            
            string jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
