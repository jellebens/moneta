using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Domain
{
    public class Price
    {
        public Price(decimal amount, string currency) {
            Amount = amount;
            Currency = currency;
        }

        public string Currency { get; private set; }

        public decimal Amount { get; private set; }
    }
}
