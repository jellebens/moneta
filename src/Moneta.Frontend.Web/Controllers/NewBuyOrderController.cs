using Microsoft.AspNetCore.Mvc;
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
            if (!this.ModelState.IsValid) {
                return View(model);
            }

            var importTransactionCommand = new
            {
                Id = Guid.NewGuid(),
                Account = model.SelectedAccount
            };

            HttpClient client = new HttpClient();

            StringContent data = new StringContent(JsonConvert.SerializeObject(importTransactionCommand), Encoding.UTF8, "application/json");
            try
            {
                var x = await client.PostAsync(_Configuration["IMPORT_TRANSACTIONS_SERVICE"], data);

                if (x.IsSuccessStatusCode)
                {
                    //Post and redirect
                    return RedirectToAction("Index", "Home");
                }
            }catch(Exception exc){
                _Logger.LogCritical(exc, "Error while trying to import transaction");
            }
            

            return View(model);
        }

        [HttpGet]
        public IActionResult GetAccounts() {
            Thread.Sleep(100);

            var accounts = new[] {
                    new { Id = "A85D51A3-C86F-447D-B30A-C251134CBE27", Name = "Test Account", Currency = "EUR" },
                    new { Id = "503E6486-6127-4EB4-B208-910C0DBD1796", Name = "Second Test Account", Currency = "USD" },
        }   ;
            return new JsonResult(accounts);
        }

    }
}
