using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TransactionService.Contracts.Data;

namespace TransactionService.Services
{
    public interface IAccountsService : IService
    {
        Task<AccountInfo> GetAsync(Guid id);

    }
    public class AccountsService : ServiceBase, IAccountsService
    {

        private readonly ILogger<AccountsService> _Logger;

        public AccountsService(IConfiguration configuration, HttpClient client, ILogger<AccountsService> logger) : base(configuration, client)
        {
            _Logger = logger;

            _Client.BaseAddress = new Uri(configuration["ACCOUNTS_SERVICE"]);
        }

        public async Task<AccountInfo> GetAsync(Guid id)
        {
            HttpResponseMessage response = await _Client.GetAsync($"/accounts/{id}");
            
            string result = await response.Content.ReadAsStringAsync();

            AccountInfo account = JsonConvert.DeserializeObject<AccountInfo>(result);

            return account;
        }
    }

}
