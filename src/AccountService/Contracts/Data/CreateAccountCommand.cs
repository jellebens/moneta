using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Contracts.Data
{
    public class CreateAccountCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }

    }
}
