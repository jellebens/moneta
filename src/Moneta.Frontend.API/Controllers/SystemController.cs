using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/sys")]
    [ApiController]
    [AllowAnonymous]
    public class SystemController : ControllerBase
    {
        [HttpGet]
        [Route("live")]
        public ActionResult IsLive()
        {
            return Ok();
        }

        [HttpGet]
        [Route("ready")]
        public ActionResult IsReady() {
            return Ok();
        }
    }
}
