using AccountService.Contracts.Data;
using AccountService.Domain;
using AccountService.Sql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moneta.Core.Jwt;
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
            
            Claim id = User.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.UserName);

            _Logger.LogInformation($"Getting Accounts for {id}");

            var accounts = _AccountsDbContext.Accounts.Where(a => a.Owner == id.Value).Select(a => new AccountInfo()
            {
                Currency = a.Currency,
                Id = a.Id,
                Name = a.Name
            }).OrderBy(a => a.Name);

            return Ok(accounts.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateAccountCommand command) {
            Claim nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            Claim id = User.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.UserName);

            _Logger.LogInformation($"Creating account {command.Name} for {nameClaim.Value}");

            Account account = new Account(Guid.NewGuid(), command.Name, command.Currency.ToUpper(), id.Value);

            bool accountExists = _AccountsDbContext.Accounts.Any(a => a.Name == account.Name && a.Currency == account.Currency && a.Owner == account.Owner);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) {

            Account account = _AccountsDbContext.Accounts.SingleOrDefault(a => a.Id == id);

            if (account == null) {
                return Ok();
            }

            _Logger.LogInformation($"Deleting account {account.Name}");

            _AccountsDbContext.Accounts.Remove(account);

            await _AccountsDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Index(Guid id)
        {
            Claim userId = User.Claims.FirstOrDefault(c => c.Type == MyClaimTypes.UserName);

            var account = _AccountsDbContext.Accounts.Where(a => a.Id == id && a.Owner == userId.Value)
                                                      .Select(a => new AccountInfo(){
                                                                        Currency = a.Currency,
                                                                        Id = a.Id,
                                                                        Name = a.Name
                                                                    })
                                                      .Single();

            return Ok(account);
        }
    }
}
