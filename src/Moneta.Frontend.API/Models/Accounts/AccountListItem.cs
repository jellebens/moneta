using System;

namespace Moneta.Frontend.API.Models.Accounts
{
    public class AccountListItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }
    }
}
