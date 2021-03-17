using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Contracts.Data
{
    public class CreateBuyOrderCommand
    {
        [Required]
        public int TransactionNumber { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Subtotal { get; set; }


        public decimal ExchangeRate { get; set; }


        public decimal Commission { get; set; }


        public decimal ExchangeRateFee { get; set; }


        public decimal TOB { get; set; }

        [Required]
        public decimal TotalCosts { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public Guid SelectedAccount { get; set; }
        public Guid Id { get; set; }
    }
}
