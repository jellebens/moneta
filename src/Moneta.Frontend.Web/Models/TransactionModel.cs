using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Models
{
    public class TransactionModel
    {
        [Required(ErrorMessage="Transaction Number is required")]
        public int? TransactionNumber { get; set; }

        [Required(ErrorMessage = "Transaction Symbol is required")]
        public string Symbol { get; set; }

        [Required(ErrorMessage = "Transaction Date is required")]
        public string TransactionDate { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Subtotal is required")]
        public decimal? Subtotal { get; set; }

        [Required(ErrorMessage = "Exchange Rate is required")]
        public decimal? ExchangeRate { get; set; }


        public decimal? Commission { get; set; }


        public decimal? ExchangeRateFee { get; set; }


        public decimal? TOB { get; set; }

        [Required(ErrorMessage = "Costs is required")]
        public decimal? TotalCosts { get; set; }

        [Required(ErrorMessage = "Total is required")]
        public decimal? Total { get; set; }

        [Required(ErrorMessage = "Account is required")]
        public Guid? SelectedAccount { get; set; }

        [Required(ErrorMessage = "Account Currency is required")]
        public string AccountCurrency { get; set; }

    }
}