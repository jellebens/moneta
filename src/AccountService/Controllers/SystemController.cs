using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
    [Route("system")]
    [ApiController]
    [AllowAnonymous]
    public class SystemController : ControllerBase
    {
        public SystemController()
        {
            
        }

        [HttpGet]
        [Route("live")]
        public ActionResult IsLive()
        {
            return Ok();
        }

        [HttpGet]
        [Route("ready")]
        public ActionResult IsReady()
        {
            return Ok();
        }
    }
}
