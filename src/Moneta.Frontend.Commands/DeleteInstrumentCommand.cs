using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Commands
{
    public class DeleteInstrumentCommand:ICommand
    {
        public Guid Id { get; set; }
        public Guid InstrumentId { get; set; }
        
    }
}
