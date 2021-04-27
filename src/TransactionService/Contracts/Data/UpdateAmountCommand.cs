using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Contracts.Data
{
    public class UpdateAmountCommand
    {
        public string Currency { get; internal set; }
        public decimal Price { get; internal set; }
        public int Quantity { get; internal set; }
        public decimal Exchangerate { get; internal set; }
    }
}
