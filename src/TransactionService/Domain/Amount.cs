using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Domain
{
    public class Amount
    {
        public Amount(Guid id, int quantity, decimal price, string currency, decimal exchangerate)
        {
            Id = id;
            Quantity = quantity;
            Price = price;
            Currency = currency;
            Exchangerate = exchangerate;
        }

        public Guid Id { get; protected set; }

        public int Quantity { get; protected set; }
        public decimal Price { get; protected set; }
        public string Currency { get; protected set; }
        public decimal Exchangerate { get; protected set; }
    }
}
