using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Domain
{
    public class Account
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public string Owner { get; set; }

    }
}
