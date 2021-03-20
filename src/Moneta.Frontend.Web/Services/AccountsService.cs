using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    }
    public class AccountsService : IAccountsService
    {
        private readonly HttpClient _Client;

        public AccountsService(IConfiguration configuration, HttpClient client)
        {
            _Client = client;
            _Client.BaseAddress = new Uri(configuration["ACCOUNTS_SERVICE"]);
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
