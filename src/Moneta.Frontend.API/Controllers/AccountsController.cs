using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Moneta.Core;
using Moneta.Core.Jwt;
using Moneta.Frontend.API.Bus;
using Moneta.Frontend.API.Hubs;
using Moneta.Frontend.API.Services;
using Moneta.Frontend.Commands;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _Logger;
        private readonly IHubContext<CommandHub> _Hub;
        private readonly IAccountsService _AccountService;
        private readonly IBus _Bus;
        private readonly IJwtTokenBuilder _JwtTokenBuilder;

        public AccountsController(ILogger<AccountsController> logger, IHubContext<CommandHub> hub, IAccountsService accountService, IBus bus, IJwtTokenBuilder jwtTokenBuilder)
        {
            _Logger = logger;
            _Hub = hub;
            _AccountService = accountService;
            _Bus = bus;
            _JwtTokenBuilder = jwtTokenBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            var accounts = await _AccountService.GetAsync(this.User);

            return Ok(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateAccountCommand createAccount)
        {
            string token = _JwtTokenBuilder.Build(this.User);
            

            await _Bus.SendAsync(Queues.Frontend.Commands, token, createAccount);

            CommandStatus status = CommandStatus.Queue(createAccount.Id);
            
            await _Hub.Clients.All.SendAsync(createAccount.Id.ToString(), status);


            return Ok();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            DeleteAccountCommand deleteAccount = new DeleteAccountCommand() { Id=Guid.NewGuid(), AccountId = id };
            string token = _JwtTokenBuilder.Build(this.User);
            
            await _Bus.SendAsync(Queues.Frontend.Commands, token, deleteAccount);

            return Ok();
        }
    }
}
