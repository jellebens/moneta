using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly IConfiguration _Configuration;

        public NewBuyOrderController(ILogger<NewBuyOrderController> logger, IConfiguration configuration)
        {
            _Logger = logger;
            _Configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(TransactionModel model)
        {
            if (!this.ModelState.IsValid)
            {
                _Logger.LogWarning("Model Invalid");
                return View(model);
            }

            var importTransactionCommand = new
            {
                Id = Guid.NewGuid(),
                TransactionNumber = model.TransactionNumber,
                Symbol = model.Symbol,
                TransactionDate = model.TransactionDate,
                Price = model.Price,
                Currency = model.Currency,
                Quantity = model.Quantity,
                Subtotal = model.Subtotal,
                ExchangeRate = model.ExchangeRate,
                Commission = model.Commission ?? 0,
                ExchangeRateFee = model.ExchangeRateFee ??0,
                TOB = model.TOB ?? 0,
                TotalCosts = model.TotalCosts,
                Total = model.Total,
                SelectedAccount = model.SelectedAccount
            };

            

            StringContent data = new StringContent(JsonConvert.SerializeObject(importTransactionCommand), Encoding.UTF8, "application/json");
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_Configuration["TRANSACTIONS_SERVICE"]);

                HttpResponseMessage response = await client.PostAsync("/buyorder/create", data);

                if (response.IsSuccessStatusCode)
                {
                    //Post and redirect
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
        public IActionResult GetAccounts()
        {
            Thread.Sleep(100);

            var accounts = new[] {
                    new { Id = "A85D51A3-C86F-447D-B30A-C251134CBE27", Name = "Test Account", Currency = "EUR" },
                    new { Id = "503E6486-6127-4EB4-B208-910C0DBD1796", Name = "Second Test Account", Currency = "USD" },
        };
            return new JsonResult(accounts);
        }

    }
}
