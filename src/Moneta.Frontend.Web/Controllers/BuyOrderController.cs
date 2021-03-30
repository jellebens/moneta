using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.Web.Models;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moneta.Frontend.WebControllers
{
    [Route("buyorder")]
    public class BuyOrderController : Controller
    {
        private readonly ILogger<BuyOrderController> _Logger;
        private readonly ITransactionsService _TransactionService;
        private readonly IAccountsService _AccountsService;
        private readonly IJwtTokenBuilder _JwtTokenBuilder;

        public BuyOrderController(ILogger<BuyOrderController> logger, ITransactionsService transactionService, IAccountsService accountsService, IJwtTokenBuilder jwtTokenBuilder)
        {
            _Logger = logger;
            _TransactionService = transactionService;
            _AccountsService = accountsService;
            _JwtTokenBuilder = jwtTokenBuilder;
        }

        [HttpGet]
        [Route("new")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new")]
        public async Task<IActionResult> Index(TransactionHeaderModel model)
        {
            if (!this.ModelState.IsValid)
            {
                _Logger.LogWarning("Model Invalid");
                return View(model);
            }

            try
            {
                _TransactionService.Authenticate(_JwtTokenBuilder.Build(User));
                HttpResponseMessage response = await _TransactionService.StartAsync(model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Amount");
                }
                else
                {
                    _Logger.LogCritical("Error while trying to create transaction: " + response.ReasonPhrase);
                }
            }
            catch (Exception exc)
            {
                _Logger.LogCritical(exc, "Error while trying to create transaction");
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult Amount() {
            return View();
        }

        [HttpPost]
        public IActionResult Amount(TransactionAmountModel model)
        {
            if (!this.ModelState.IsValid)
            {
                _Logger.LogWarning("Model Invalid");
                return View(model);
            }

            return RedirectToAction("Costs");
        }

        [HttpGet]
        [Route("accounts")]
        public async Task<IActionResult> GetAccounts()
        {

            _AccountsService.Authenticate(_JwtTokenBuilder.Build(User));

            AccountInfo[] accounts = await _AccountsService.ListAsync();

            return new JsonResult(accounts);
        }
    }
}
