using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("me")]
        public IActionResult Me() {

            var user = new
            {
                name = this.User.Claims.Single(x => x.Type.Equals("name")).Value,
                Id = this.User.Claims.Single(x => x.Type.Equals("preferred_username")).Value
            };

            return Ok(user);
        }
    }
}
