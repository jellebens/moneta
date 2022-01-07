using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Moneta.Core;
using Moneta.Frontend.API.Bus;
using Moneta.Frontend.API.Services;
using Moneta.Frontend.Commands;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _Logger;
        private readonly IAccountsService _AccountService;
        private readonly IBus _Bus;

        public AccountsController(ILogger<AccountsController> logger, IAccountsService accountService, IBus bus)
        {
            _Logger = logger;
            _AccountService = accountService;
            _Bus = bus;
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
            await _Bus.SendAsync(Queues.Frontend.Commands, createAccount);

            return Ok();
        }

    }
}
