using Moneta.Frontend.CommandProcessor.Services;
using Moneta.Frontend.Commands.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Handlers.Transactions
{
    public class NewTransferCommandHandler : ICommandHandler<NewTransferCommand>
    {
        private readonly ITransactionService _TransactionService;

        public NewTransferCommandHandler(ITransactionService transactionService)
        {
            this._TransactionService = transactionService;
        }
        public async Task Execute(string token, NewTransferCommand command)
        {
            _TransactionService.Authenticate(token);
            await _TransactionService.Transfer(command);
        }
    }
}
