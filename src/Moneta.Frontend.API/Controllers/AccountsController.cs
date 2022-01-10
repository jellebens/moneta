using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Moneta.Core;
using Moneta.Core.Jwt;
using Moneta.Frontend.API.Bus;
using Moneta.Frontend.API.Services;
using Moneta.Frontend.Commands;
using System.Diagnostics;
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
        private readonly IJwtTokenBuilder _JwtTokenBuilder;

        public AccountsController(ILogger<AccountsController> logger, IAccountsService accountService, IBus bus, IJwtTokenBuilder jwtTokenBuilder)
        {
            _Logger = logger;
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
            //https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
            await _Bus.SendAsync(Queues.Frontend.Commands, token, createAccount);

            return Ok();
        }

    }
}
