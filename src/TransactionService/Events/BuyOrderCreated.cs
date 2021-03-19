using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Events
{
    public class BuyOrderCreated
    {
        public int TransactionNumber { get; set; }

        public string Symbol { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public int Quantity { get; set; }

        public decimal Subtotal { get; set; }


        public decimal ExchangeRate { get; set; }


        public decimal Commission { get; set; }


        public decimal ExchangeRateFee { get; set; }


        public decimal TOB { get; set; }

        public decimal TotalCosts { get; set; }

        public decimal Total { get; set; }

        public Guid SelectedAccount { get; set; }
        public Guid Id { get; set; }
    }
}
