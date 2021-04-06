using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionService.Sql;

namespace TransactionService.Controllers
{
    [Route("system")]
    [ApiController]
    [AllowAnonymous]
    public class SystemController : ControllerBase
    {
        private readonly ILogger<SystemController> _Logger;
        private readonly TransactionsDbContext _TransactionsDb;

        public SystemController(ILogger<SystemController> logger, TransactionsDbContext transactionsDb)
        {
            _Logger = logger;
            _TransactionsDb = transactionsDb;
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
            if (!_TransactionsDb.Database.CanConnect())
            {
                _Logger.LogCritical("Cant connect to database");

                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Can't Connect to database");
            };
            return Ok();
        }
    }
}
