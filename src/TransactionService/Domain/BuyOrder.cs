using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Domain
{
    public class BuyOrder
    {
        protected BuyOrder() { 
        
        }

        public BuyOrder(Guid id, Guid accountId, string currency, string symbol, DateTime transactionDate, int transactionNumber, string userId)
        {
            Id = id;
            AccountId = accountId;
            Currency = currency;
            UserId = userId;
            Symbol = symbol;
            Date = transactionDate;
            Number = transactionNumber;
        }

        public Guid Id { get; protected set; }

        public Guid AccountId { get; protected set; }

        public string UserId { get; protected set; }

        public string Symbol { get; set; }

        public long Number { get; protected set; }
        
        public string Currency { get; set; }

        public DateTime Date { get; protected set; }


    }
}
