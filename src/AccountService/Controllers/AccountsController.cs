using AccountService.Contracts.Data;
using AccountService.Domain;
using AccountService.Sql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {

        private readonly ILogger<AccountsController> _Logger;
        private readonly AccountsDbContext _AccountsDbContext;

        public AccountsController(ILogger<AccountsController> logger, AccountsDbContext accountsDbContext)
        {
            _Logger = logger;
            _AccountsDbContext = accountsDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            AccountInfo[] accounts = new AccountInfo[] {
                    new AccountInfo { Id = new Guid("A85D51A3-C86F-447D-B30A-C251134CBE27"), Name = "Test Account", Currency = "EUR" },
                    new AccountInfo { Id = new Guid("503E6486-6127-4EB4-B208-910C0DBD1796"), Name = "Second Test Account", Currency = "USD" },
            };

            return Ok(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateAccountCommand command) {
            Claim nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            Claim id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            _Logger.LogInformation($"Creating account {command.Name} for {nameClaim.Value}");

            Account account = new Account(Guid.NewGuid(), command.Name, command.Currency, id.Value);

            bool accountExists =  _AccountsDbContext.Accounts.Any(a => a.Name == account.Name && a.Currency == account.Currency && a.Owner == account.Owner);

            if (!accountExists) {
                await _AccountsDbContext.Accounts.AddAsync(account);
                await _AccountsDbContext.SaveChangesAsync();
            }
            else
            {
                _Logger.LogInformation("Account allready exists, not creating a new one");
            }

            return Ok();
        }
    }
}
