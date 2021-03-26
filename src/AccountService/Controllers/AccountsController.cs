﻿using AccountService.Contracts.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {

        private readonly ILogger<AccountsController> _Logger;

        public AccountsController(ILogger<AccountsController> logger)
        {
            _Logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            _Logger.LogInformation("User = " + User.Claims);

            AccountInfo[] accounts = new AccountInfo[] {
                    new AccountInfo { Id = new Guid("A85D51A3-C86F-447D-B30A-C251134CBE27"), Name = "Test Account", Currency = "EUR" },
                    new AccountInfo { Id = new Guid("503E6486-6127-4EB4-B208-910C0DBD1796"), Name = "Second Test Account", Currency = "USD" },
            };

            return Ok(accounts);
        }
    }
}
