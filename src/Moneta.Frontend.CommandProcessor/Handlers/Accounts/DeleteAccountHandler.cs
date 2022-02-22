using Moneta.Frontend.CommandProcessor.Services;
using Moneta.Frontend.Commands.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Handlers.Accounts
{
    public class DeleteAccountHandler : ICommandHandler<DeleteAccountCommand>
    {
        private readonly ILogger<DeleteAccountHandler> _Logger;
        private readonly IAccountsService _AccountService;

        public DeleteAccountHandler(IAccountsService accountService, ILogger<DeleteAccountHandler> logger)
        {
            _AccountService = accountService;
            _Logger = logger;
        }

        public async Task Execute(string token, DeleteAccountCommand command)
        {
            _AccountService.Authenticate(token);
            await _AccountService.Delete(command);
        }
    }
}
