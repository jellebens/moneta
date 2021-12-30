using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Moneta.Frontend.API.Services;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _Logger;
        private readonly IAccountsService _AccountService;
        
        public AccountsController(ILogger<AccountsController> logger, IAccountsService accountService)
        {
            _Logger = logger;
            _AccountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accounts = await _AccountService.GetAsync(this.User);
            
            return Ok(accounts);
        }
    }
}
