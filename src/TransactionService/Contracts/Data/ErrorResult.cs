using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Contracts.Data
{
    public class ErrorResult
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
