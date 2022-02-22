using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TransactionService.Contracts.Data;
using TransactionService.Sql;
using System.Linq;
using TransactionService.Domain;
using TransactionService.Bus;
using Moneta.Events.Transactions;
using System;
using TransactionService.Services;
using System.Threading.Tasks;

namespace TransactionService.Controllers
{
    [Route("cash")]
    [ApiController]
    public class CashController : ControllerBase
    {
        private readonly ILogger<CashController> _Logger;
        private readonly TransactionsDbContext _TransactionDbContext;
        private readonly IBus _Bus;
        private readonly IAccountsService _AccountsService;

        public CashController(ILogger<CashController> logger, TransactionsDbContext transactionDbContext, IBus bus, IAccountsService accountsService)
        {
            _Logger = logger;
            _TransactionDbContext = transactionDbContext;
            _Bus = bus;
            _AccountsService = accountsService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(DepositCommand deposit) {
            _Logger.LogInformation("Starting Transfer");
            
            Currency c = _TransactionDbContext.Currencies.SingleOrDefault(c => c.Symbol.ToUpper() == deposit.Currency.ToUpper());

            if (c == null) {
                c = new Currency() {
                    Symbol = deposit.Currency.ToUpper()
                };

                _TransactionDbContext.Currencies.Add(c);
            }
            string token = this.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);

            _AccountsService.Authenticate(token);

            AccountInfo account = await _AccountsService.GetAsync(deposit.AccountId);
            _Logger.LogInformation($"Transfer money to {account.Name}");

            if (deposit.Currency != account.Currency) {
                return BadRequest("Deposit currency and account currency do not match");
            }

            CashTransfer cashTransfer = new CashTransfer();
            cashTransfer.Currency = c;
            cashTransfer.Date = deposit.Date;
            cashTransfer.Amount = deposit.Amount;
            cashTransfer.AccountId = account.Id;

            _TransactionDbContext.Add(cashTransfer);

            _TransactionDbContext.SaveChangesAsync();

            if (deposit.Amount > 0) {
                _Bus.SendAsync(new CashDepositedEvent() { 
                    AccountId = deposit.AccountId,
                    Amount = deposit.Amount,
                    Currency = deposit.Currency,
                    Date = deposit.Date,
                    Id = Guid.NewGuid(),

                });
            }
            else
            {
                _Bus.SendAsync(new CashWithdrawnEvent() {
                    AccountId = deposit.AccountId,
                    Amount = Math.Abs(deposit.Amount),
                    Currency = deposit.Currency,
                    Date = deposit.Date,
                    Id = Guid.NewGuid(),
                });
            }

            return StatusCode(StatusCodes.Status201Created);

        }
    }
}
