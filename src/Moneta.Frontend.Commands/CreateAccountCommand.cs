using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Commands
{
    public class CreateAccountCommand: ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Currency { get; set; }

    }
}
