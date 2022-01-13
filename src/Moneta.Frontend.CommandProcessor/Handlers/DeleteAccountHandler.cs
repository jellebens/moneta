﻿using Moneta.Frontend.CommandProcessor.Services;
using Moneta.Frontend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Handlers
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

        public void Execute(string token, DeleteAccountCommand command)
        {
            _AccountService.Delete(command, token);
        }
    }
}
