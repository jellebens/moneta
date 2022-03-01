using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionService.Bus;
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
        private readonly IBus _Bus;

        public SystemController(ILogger<SystemController> logger, TransactionsDbContext transactionsDb, IBus bus)
        {
            _Logger = logger;
            _TransactionsDb = transactionsDb;
            this._Bus = bus;
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

            if (!_Bus.IsConnected()) {
                _Logger.LogCritical("Cant connect to RabbitMQ");

                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Can't Connect to RabbitMQ");
            }
            return Ok();
        }
    }
}
