using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Domain
{
    public class Account
    {
        private Account() { 
        
        }

        public Account(Guid id, string name, string currency, string owner) { 
            Id = id;
            Name = name;
            Currency = currency;
            Owner = owner;
        }

        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public string Currency { get; protected  set; }

        public string Owner { get; protected set; }

    }
}
