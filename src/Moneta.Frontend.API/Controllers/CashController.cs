using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moneta.Core;
using Moneta.Frontend.API.Bus;
using Moneta.Frontend.API.Hubs;
using Moneta.Frontend.Commands;
using Moneta.Frontend.Commands.Transactions;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/cash")]
    [ApiController]
    public class CashController : ControllerBase
    {
        private readonly ILogger<CashController> _Logger;
        private readonly IHubContext<CommandHub> _Hub;
        private readonly IBus _Bus;

        public CashController(ILogger<CashController> logger, IHubContext<CommandHub> hub, IBus bus)
        {
            _Logger = logger;
            _Hub = hub;
            _Bus = bus;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(NewTransferCommand transferCommand) {

            var token = this.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);

            await _Bus.SendAsync(Queues.Frontend.Commands, token, transferCommand);

            CommandStatus status = CommandStatus.Queue(transferCommand.Id);

            await _Hub.Clients.All.SendAsync(transferCommand.Id.ToString(), status);

            return StatusCode(StatusCodes.Status202Accepted);
        }
    }
}
