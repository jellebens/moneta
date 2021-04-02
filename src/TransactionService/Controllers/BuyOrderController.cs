using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TransactionService.Contracts.Data;
using TransactionService.Domain;
using TransactionService.Events;
using TransactionService.Sql;
using static Confluent.Kafka.ConfigPropertyNames;

namespace TransactionService.Controllers
{
    [Route("buyorder")]
    [ApiController]
    public class BuyOrderController : ControllerBase
    {
        private readonly ILogger<BuyOrderController> _Logger;
        private readonly IConfiguration _Configuration;
        private readonly TransactionsDbContext _TransactionsDbContext;

        public BuyOrderController(ILogger<BuyOrderController> logger, IConfiguration configuration, TransactionsDbContext transactionsDbContext)
        {
            _Logger = logger;
            _Configuration = configuration;
            _TransactionsDbContext = transactionsDbContext;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody]StartBuyOrderCommand createBuyOrder, CancellationToken cancellationToken) {
            Guid id = Guid.NewGuid();
            _Logger.LogInformation($"Creating Buy Order with Id: {id} and number {createBuyOrder.TransactionNumber}");

            Claim userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (_TransactionsDbContext.BuyOrders.Any(x => x.AccountId == createBuyOrder.AccountId && x.Number == createBuyOrder.TransactionNumber)) {
                return StatusCode(StatusCodes.Status409Conflict, new ErrorResult { Code = "duplicate_buy_order", Message = $"Buyorder with number {createBuyOrder.TransactionNumber} for this account allready exists"});
            }
            
            BuyOrder buyOrder = new BuyOrder(id, createBuyOrder.AccountId, createBuyOrder.Currency.ToUpper(), createBuyOrder.Symbol, createBuyOrder.TransactionDate, createBuyOrder.TransactionNumber, userId.Value);

            await _TransactionsDbContext.BuyOrders.AddAsync(buyOrder);

            await _TransactionsDbContext.SaveChangesAsync(cancellationToken);

            _Logger.LogInformation($"Created Buy Order with Id: {id} and number {createBuyOrder.TransactionNumber}");
            return Ok();
        }
    }
}
