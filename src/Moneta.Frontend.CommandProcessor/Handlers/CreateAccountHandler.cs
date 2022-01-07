using Moneta.Frontend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Handlers
{
    internal class CreateAccountHandler : ICommandHandler<CreateAccountCommand>
    {
        private readonly ILogger<CreateAccountHandler> _Logger;

        public CreateAccountHandler(ILoggerFactory logger)
        {
            _Logger = logger.CreateLogger<CreateAccountHandler>();
        }
        public void Execute(CreateAccountCommand command)
        {
            _Logger.LogInformation($"Creating account {command.Name} with {command.Currency}");
            //Console.WriteLine($"Creating account {command.Name} with {command.Currency}");
        }
    }
}
