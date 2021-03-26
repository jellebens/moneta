using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Services
{
    public class AccountInfo {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }
    }

    public interface IAccountsService
    {
        Task<AccountInfo[]> ListAsync();
        void AddBearer(string jwtToken);
    }
    public class AccountsService : IAccountsService
    {
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _Client;
        private readonly ILogger<AccountsService> _Logger;

        public AccountsService(IConfiguration configuration, HttpClient client, ILogger<AccountsService> logger)
        {
            _Client = client;
            _Logger = logger;
            _Configuration = configuration;

            _Client.BaseAddress = new Uri(configuration["ACCOUNTS_SERVICE"]);
        }

        public void AddBearer(string jwtToken)
        {
            _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, jwtToken);
        }

        public async Task<AccountInfo[]> ListAsync()
        {
            
            HttpResponseMessage response = await _Client.GetAsync("/accounts");

            string result = await response.Content.ReadAsStringAsync();

            AccountInfo[] list = JsonConvert.DeserializeObject<AccountInfo[]>(result);

            return list;
        }
    }
}
