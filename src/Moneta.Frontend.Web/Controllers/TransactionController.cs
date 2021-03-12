using Microsoft.AspNetCore.Mvc;
using Moneta.Frontend.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.WebControllers
{
    public class TransactionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ImportTransactionModel model)
        {
            if (!this.ModelState.IsValid) {
                return View(model);
            }
            //Post and redirect
            return RedirectToAction("Index","Home");
        }
    }
}
