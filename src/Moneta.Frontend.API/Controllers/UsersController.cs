using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moneta.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _Logger;

        public UsersController(ILogger<UsersController> logger)
        {
            this._Logger = logger;
        }

        [HttpGet("me")]
        public IActionResult Me() {
            using (Activity getUserActivity = new ActivitySource(Telemetry.Source).StartActivity("Get User")) {
                try
                {
                    var user = new
                    {
                        name = this.User.Claims.Single(x => x.Type.Equals("name")).Value,
                        Id = this.User.Claims.Single(x => x.Type.Equals("preferred_username")).Value
                    };

                    _Logger.LogInformation($"{this.User.IsInRole("admin")}");

                    return Ok(user);
                }
                catch(Exception)
                {
                    
                    _Logger.LogInformation($"Logged on as: {this.User.Identity.Name}");
                    _Logger.LogInformation($"Printing Claims");
                    ClaimsPrincipal principal = this.User as ClaimsPrincipal;
                    if (null != principal)
                    {
                        foreach (Claim claim in principal.Claims)
                        {
                            _Logger.LogInformation("CLAIM TYPE: " + claim.Type + "; CLAIM VALUE: " + claim.Value);
                        }
                    }

                    return BadRequest();

                }
                
            }
            
        }
    }
}
