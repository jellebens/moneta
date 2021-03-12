using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImportTransactionService.Contracts.Data
{
    public class ImportTransactionsCommand
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Account { get; set; }

        [Required]
        public string Transactions { get; set; }
    }
}
