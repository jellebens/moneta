﻿using Microsoft.Extensions.Configuration;
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

        public JwtTokenBuilder(IConfiguration configuration)
        {
            this._Configuration = configuration;
        }
        public string Build(ClaimsPrincipal principal)
        {

            //string name = principal.Claims.FirstOrDefault(c => );
            string name = principal.Claims.FirstOrDefault(c => c.Type.Equals("name", StringComparison.CurrentCultureIgnoreCase)).Value;
            string id = principal.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username", StringComparison.CurrentCultureIgnoreCase)).Value; ;

            var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT_SECRET"]));

            var myIssuer = "jellebens.ddns.net";
            var myAudience = "https://jellebens.ddns.net";

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Name, name)
                }),
                NotBefore = DateTime.UtcNow.AddMinutes(-15),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
