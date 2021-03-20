using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.Web.Clients;
using Moneta.Frontend.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moneta.Frontend.WebControllers
{
    public class NewBuyOrderController : Controller
    {
        private readonly ILogger<NewBuyOrderController> _Logger;
        private readonly ITransactionsService _TransactionService;
        private readonly IAccountsService _AccountsService;

        public NewBuyOrderController(ILogger<NewBuyOrderController> logger, ITransactionsService transactionService, IAccountsService accountsService)
        {
            _Logger = logger;
            _TransactionService = transactionService;
            _AccountsService = accountsService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(TransactionModel model)
        {
            if (!this.ModelState.IsValid)
            {
                _Logger.LogWarning("Model Invalid");
                return View(model);
            }

            try
            {
                HttpResponseMessage response = await _TransactionService.CreateAsync(model);

                if (response.IsSuccessStatusCode)
                {
                    //redirect
                    return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> GetAccounts()
        {
            AccountInfo[] accounts = await _AccountsService.ListAsync();

            return new JsonResult(accounts);
        }

    }
}
