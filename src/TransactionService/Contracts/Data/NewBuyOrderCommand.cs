using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Contracts.Data
{
    public class NewBuyOrderCommand
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public int TransactionNumber { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string Currency { get; internal set; }
    }
}
