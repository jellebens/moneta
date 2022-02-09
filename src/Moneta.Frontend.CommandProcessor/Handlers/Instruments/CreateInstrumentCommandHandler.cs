using Moneta.Frontend.CommandProcessor.Services;
using Moneta.Frontend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Handlers.Instruments
{
    public class CreateInstrumentCommandHandler : ICommandHandler<CreateInstrumentCommand>
    {
        private readonly IInstrumentService _InstrumentService;

        public CreateInstrumentCommandHandler(IInstrumentService instrumentService)
        {
            _InstrumentService = instrumentService;
        }
        public void Execute(string token, CreateInstrumentCommand command)
        {
            _InstrumentService.Authenticate(token);
            _InstrumentService.Create(command);
        }
    }
}
