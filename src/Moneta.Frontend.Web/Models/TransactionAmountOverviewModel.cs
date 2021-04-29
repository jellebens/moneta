using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Models
{
    public class TransactionAmountOverviewModel
    {
        public string Price { get; set; }
        public string PriceCurrency { get; set; }

        public string Quantity { get; set; }

        public string ExchangeRate { get; set; }

        public string Total { get; set; }

        public string Currency { get; set; }
    }
}
