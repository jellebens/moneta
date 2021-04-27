using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Models
{
    public class TransactionCostsModel
    {

        [Required(ErrorMessage = "Commission is required")]
        public decimal Commission { get; set; }

        [Required(ErrorMessage = "Commission is required")]
        public decimal CostExchangeRate { get; set; }

        [Required(ErrorMessage = "Stock Market Tax is required")]
        public decimal StockMarketTax { get; set; }
    }
}
