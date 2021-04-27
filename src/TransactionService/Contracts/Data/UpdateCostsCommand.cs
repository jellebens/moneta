using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Contracts.Data
{
    public class UpdateCostsCommand
    {
        public decimal Commision { get; internal set; }
        public decimal CostExchangerate { get; internal set; }
        public decimal StockMarketTax { get; internal set; }
    }
}
