using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneta.Frontend.API.Bus;
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
        private readonly IBus bus;

        public SystemController(IBus bus)
        {
            this.bus = bus;
        }

        [HttpGet]
        [Route("live")]
        public ActionResult IsLive()
        {
            return Ok();
        }

        [HttpGet]
        [Route("ready")]
        public ActionResult IsReady() {
            if (bus.IsConnected()) { 
                return Ok();
            }
            return BadRequest("Bus Not connected");
        }
    }
}
