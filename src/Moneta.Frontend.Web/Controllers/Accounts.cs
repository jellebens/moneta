using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Controllers
{
    [Route("accounts")]
    public class Accounts : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
