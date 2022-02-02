using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.API.Hubs;
using Moneta.Frontend.Commands;
using System;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ILogger<CommandsController> _Logger;
        private readonly IHubContext<CommandHub> _Hub;

        public CommandsController(ILogger<CommandsController> logger, IHubContext<CommandHub> hub)
        {
            this._Logger = logger;
            this._Hub = hub;
        }
    }
}
