using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Models
{
    public class TransactionCostsOverviewModel
    {

        public string Commission { get; set; }

        public string CostExchangeRate { get; set; }

        public string StockMarketTax { get; set; }

        public string Total { get; set; }

        public string Currency { get; set; }
    }
}
