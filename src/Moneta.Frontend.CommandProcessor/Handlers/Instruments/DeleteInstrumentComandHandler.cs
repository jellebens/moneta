using Moneta.Frontend.CommandProcessor.Services;
using Moneta.Frontend.Commands.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Handlers.Instruments
{

    class DeleteInstrumentComandHandler : ICommandHandler<DeleteInstrumentCommand>
    {
        private readonly IInstrumentService _InstrumentService;

        public DeleteInstrumentComandHandler(IInstrumentService instrumentService)
        {
            _InstrumentService = instrumentService;
        }
        public async Task Execute(string token, DeleteInstrumentCommand command)
        {
            _InstrumentService.Authenticate(token);
            await _InstrumentService.Delete(command);
        }
    }
}
