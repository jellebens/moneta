using Microsoft.AspNetCore.Mvc;
using Moneta.Frontend.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Controllers
{
    [Route("accounts")]
    public class Accounts : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromBody]NewAccountModel model)
        {
            
            return Ok();
        }
    }
}
