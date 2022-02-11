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
        private readonly ILogger<CreateInstrumentCommandHandler> _Logger;

        public CreateInstrumentCommandHandler(IInstrumentService instrumentService, ILogger<CreateInstrumentCommandHandler> logger)
        {
            _InstrumentService = instrumentService;
            _Logger = logger;
        }
        public void Execute(string token, CreateInstrumentCommand command)
        {
            _Logger.LogInformation($"Creating instrument with symbol: {command.Symbol}");
            try
            {
                _InstrumentService.Authenticate(token);
                _InstrumentService.Create(command);
            }
            catch (Exception exc)
            {
                _Logger.LogCritical($"Error Creating instrument: {exc.Message}");
                throw;
            }
            
            
        }
    }
}
