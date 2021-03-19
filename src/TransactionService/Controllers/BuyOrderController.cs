using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TransactionService.Contracts.Data;
using TransactionService.Events;
using static Confluent.Kafka.ConfigPropertyNames;

namespace TransactionService.Controllers
{
    [Route("buyorder")]
    [ApiController]
    public class BuyOrderController : ControllerBase
    {
        private readonly ILogger<BuyOrderController> _Logger;
        private readonly IConfiguration _Configuration;

        public BuyOrderController(ILogger<BuyOrderController> logger, IConfiguration configuration)
        {
            _Logger = logger;
            _Configuration = configuration;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateBuyOrderCommand createBuyOrder, CancellationToken cancellationToken) {
            _Logger.LogInformation($"Creating Buy Order with Id: {createBuyOrder.Id} and number {createBuyOrder.TransactionNumber}");

            

            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _Configuration["BOOTSTRAP_SERVERS"],
                ClientId = Dns.GetHostName()
            };




            using (var producer = new ProducerBuilder<Guid, BuyOrderCreated>(config).Build()) {
                Message<Guid, BuyOrderCreated> msg =  new Message<Guid, BuyOrderCreated>()
                {
                    Key = createBuyOrder.SelectedAccount,
                    Value = new BuyOrderCreated {

                    }
                };

                var deliveryResult = await producer.ProduceAsync("transactions", msg, cancellationToken);
            }

                _Logger.LogInformation($"Finished creating Buy Order with Id: {createBuyOrder.Id} and number {createBuyOrder.TransactionNumber}");

            return Ok();
        }
    }
}
