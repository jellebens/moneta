using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Contracts.Data
{
    public class AccountInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }
    }
}
