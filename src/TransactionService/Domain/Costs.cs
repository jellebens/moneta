using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Domain
{
    public class Cost
    {
        
        public Cost(Guid id, decimal amount, string type)
        {
            this.Id = id;
            this.Amount = amount;
            this.Type = type;
        }

        public decimal Amount { get; set; }
        public string Type { get; set; }

        public Guid Id { get; internal set; }
        public BuyOrder Buyorder { get; internal set; }
    }
}
