using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Controllers
{
    [AllowAnonymous, Route("auth")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [Route("login")]
        public IActionResult Index()
        {
            
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("Index","Home") };
            
            _logger.LogInformation("Redirecting to: " + properties.RedirectUri);

            //return Challenge(properties, FacebookDefaults.AuthenticationScheme);
            return Ok();
        }

        [Route("response")]
        [Authorize]
        public IActionResult FacebookResponse()
        {
            try
            {
                var claims = User.Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

                return Json(claims);
            }
            catch (Exception exc) {
                return StatusCode(500, exc.Message);
            }
            
        }
    }
}
