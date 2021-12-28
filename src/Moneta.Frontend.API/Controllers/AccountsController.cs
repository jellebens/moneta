using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _Logger;

        public AccountsController(ILogger<AccountsController> logger)
        {
            _Logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var accounts = new[]{
                                    new { name = "mock account 1", currency = "EUR"},
                                    new { name = "mock account 2", currency = "USD"},
                                };

            return Ok(accounts);
        }
    }
}
