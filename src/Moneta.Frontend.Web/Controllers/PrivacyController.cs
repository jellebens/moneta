using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Controllers
{
    [AllowAnonymous]
    [Route("privacy")]
    public class PrivacyController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("delete")]
        public IActionResult Delete() {
            return Ok();
        }


    }
}
