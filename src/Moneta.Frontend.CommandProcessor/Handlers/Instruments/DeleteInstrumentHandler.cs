using Moneta.Frontend.CommandProcessor.Services;
using Moneta.Frontend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Handlers.Instruments
{

    class DeleteInstrumentHandler : ICommandHandler<DeleteInstrumentCommand>
    {
        private readonly IInstrumentService _InstrumentService;

        public DeleteInstrumentHandler(IInstrumentService instrumentService)
        {
            _InstrumentService = instrumentService;
        }
        public void Execute(string token, DeleteInstrumentCommand command)
        {
            _InstrumentService.Authenticate(token);
            _InstrumentService.Delete(command);
        }
    }
}
