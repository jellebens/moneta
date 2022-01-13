﻿using Moneta.Frontend.CommandProcessor.Services;
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
        private readonly IAccountsService _AccountService;

        public CreateAccountHandler(IAccountsService accountService, ILoggerFactory logger)
        {
            _Logger = logger.CreateLogger<CreateAccountHandler>();
            _AccountService = accountService;
        }
        public void Execute(string token, CreateAccountCommand command)
        {
            _AccountService.Create(command, token);
        }
    }
}
