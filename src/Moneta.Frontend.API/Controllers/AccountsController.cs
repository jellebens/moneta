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
using Moneta.Frontend.API.Models.Accounts;
using Moneta.Frontend.API.Services;
using Moneta.Frontend.Commands;
using Moneta.Frontend.Commands.Accounts;
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

        public AccountsController(ILogger<AccountsController> logger, IHubContext<CommandHub> hub, IAccountsService accountService, IBus bus)
        {
            _Logger = logger;
            _Hub = hub;
            _AccountService = accountService;
            _Bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string token = this.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
            
            _AccountService.Authenticate(token);

            var accounts = await _AccountService.GetAsync();

            return Ok(accounts);
        }

        [HttpGet("deposits/summary/year/{id}")]
        public async Task<IActionResult> DepositsPerYear(Guid id)
        {
            string token = this.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);

            _AccountService.Authenticate(token);

            var results = await _AccountService.GetSummaryByYear(id);

            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateAccountCommand createAccount)
        {
            var token = this.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);


            await _Bus.SendAsync(Queues.Frontend.Commands, token, createAccount);

            CommandStatus status = CommandStatus.Queue(createAccount.Id);
            
            await _Hub.Clients.All.SendAsync(createAccount.Id.ToString(), status);

            return Ok();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            DeleteAccountCommand deleteAccount = new DeleteAccountCommand() { Id=Guid.NewGuid(), AccountId = id };
            var token = this.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);

            await _Bus.SendAsync(Queues.Frontend.Commands, token, deleteAccount);

            return Ok();
        }
    }
}
