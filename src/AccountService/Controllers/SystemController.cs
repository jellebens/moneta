using AccountService.Sql;
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
        private readonly ILogger<SystemController> _Logger;
        private readonly AccountsDbContext _AccountsDb;

        public SystemController(ILogger<SystemController> logger, AccountsDbContext accountsDb)
        {
            _Logger = logger;
            _AccountsDb = accountsDb;
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
            if (!_AccountsDb.Database.CanConnect()) {
                _Logger.LogCritical("Cant connect to database");

                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Can't Connect to database");
            };
            return Ok();
        }
    }
}
