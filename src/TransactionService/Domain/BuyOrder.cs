using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Domain
{
    public class BuyOrder
    {
        protected BuyOrder()
        {

        }

        public BuyOrder(Guid id, Guid accountId, string symbol, string currency, DateTime transactionDate, int transactionNumber, string userId)
        {
            Id = id;
            AccountId = accountId;
            UserId = userId;
            Symbol = symbol;
            Date = transactionDate;
            Number = transactionNumber;
            Currency = currency;
        }

        public Guid Id { get; protected set; }

        public Guid AccountId { get; protected set; }

        public string UserId { get; protected set; }

        public string Symbol { get; protected set; }

        public long Number { get; protected set; }



        public DateTime Date { get; protected set; }
        public Amount Amount { get; protected set; }

        public decimal Subtotal
        {
            get
            {
                if (this.Amount == null)
                {
                    return 0;
                }

                return Amount.Quantity * Amount.Price / Amount.Exchangerate;

            }
        }

        public decimal TotalCosts
        {
            get
            {
                decimal costs = 0;
                if (this.Costs == null) {
                    return costs;
                }

                foreach (Cost cost in Costs)
                {
                    costs += cost.Amount;
                }

                return costs;
            }
        }

        public IList<Cost> Costs { get; internal set; }
        public string Currency { get; protected set; }

        public void With(Amount amount)
        {
            this.Amount = amount;
        }

        internal void With(Cost costs)
        {
            if (this.Costs == null) {
                this.Costs = new List<Cost>();
            }

            this.Costs.Add(costs);
        }
    }
}
