using AccountService.Sql;
using Moneta.Events.Transactions;
using System.Threading.Tasks;
using System.Linq;
using AccountService.Domain;

namespace AccountService.Events.Handlers.Transactions
{
    public class CashDepositedEventHandler : IEventHandler<CashDepositedEvent>
    {
        private readonly AccountsDbContext _DbContext;

        public CashDepositedEventHandler(AccountsDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public async Task Handle(CashDepositedEvent evnt)
        {
            Deposit d = _DbContext.Deposits.SingleOrDefault(x => x.Account.Id == evnt.AccountId && x.Year == evnt.Date.Year);


            if (d == null)
            {
                Account a = _DbContext.Accounts.Single(a => a.Id == evnt.AccountId);
                d = new Deposit()
                {
                    Account = a,
                    Year = evnt.Date.Year,
                    Amount = evnt.Amount,
                };
                _DbContext.Deposits.Add(d);
            }
            else {
                d.Amount += evnt.Amount;
            }

            _DbContext.SaveChanges();
        }
    }
}
