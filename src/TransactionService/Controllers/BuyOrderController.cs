using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionService.Contracts.Data;

namespace TransactionService.Controllers
{
    [Route("buyorder")]
    [ApiController]
    public class BuyOrderController : ControllerBase
    {
        private readonly ILogger<BuyOrderController> _Logger;

        public BuyOrderController(ILogger<BuyOrderController> logger)
        {
            _Logger = logger;
        }

        [HttpPost("create")]
        public void Create([FromBody]CreateBuyOrderCommand createBuyOrder) {
            _Logger.LogInformation($"Creating Buy Order with Id: {createBuyOrder.Id} and number {createBuyOrder.TransactionNumber}");

            _Logger.LogInformation($"Finished creating Buy Order with Id: {createBuyOrder.Id} and number {createBuyOrder.TransactionNumber}");
        }
    }
}
