using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Contracts.Data
{
    public class UpdateAmountCommand
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Exchangerate { get; set; }
    }
}
