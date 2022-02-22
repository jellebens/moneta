using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Commands.Accounts
{
    public class DeleteAccountCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
    }
}
