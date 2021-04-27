using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Contracts.Data
{
    public class UpdateCostsCommand
    {
        public Guid Id { get; set; }
        public decimal Commision { get; set; }
        public decimal CostExchangerate { get; set; }
        public decimal StockMarketTax { get; set; }
    }
}
