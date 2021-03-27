using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.Web.Models;
using Moneta.Frontend.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Controllers
{
    [Route("accounts")]
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _Logger;
        private readonly IAccountsService _AccountService;
        private readonly IJwtTokenBuilder _JwtTokenBuilder;

        public AccountsController(ILogger<AccountsController> logger, IAccountsService accountService, IJwtTokenBuilder jwtTokenBuilder)
        {
            _Logger = logger;
            _AccountService = accountService;
            _JwtTokenBuilder = jwtTokenBuilder;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody]NewAccountModel model)
        {
            _AccountService.Authenticate(_JwtTokenBuilder.Build(User));
            HttpResponseMessage response = await _AccountService.CreateAccountAsync(model);
            if (!response.IsSuccessStatusCode) {
                _Logger.LogError("Error creating acount: " + response.ReasonPhrase);
                return BadRequest();
            };

            return Ok();
        }
    }
}
