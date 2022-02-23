using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Moneta.Core.Jwt;
using Moneta.Frontend.API.Models.Accounts;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Services
{

    public interface IAccountsService: IService
    {
        Task<AccountListItem[]> GetAsync();
        Task<AccountSummaryByYear[]> GetSummaryByYear(Guid id);
    }


    public class AccountsService : ServiceBase, IAccountsService
    {

        private readonly ILogger<AccountsService> _Logger;
        
        public AccountsService(IConfiguration configuration, HttpClient client, ILogger<AccountsService> logger): base(configuration, client)
        {
            _Logger = logger;
        }

        public async Task<AccountListItem[]> GetAsync()
        {
            HttpResponseMessage response = await _Client.GetAsync($"/accounts");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                AccountListItem[] accounts = JsonConvert.DeserializeObject<AccountListItem[]>(result);

                return accounts;
            }

            _Logger.LogError($"Error retrieving accounts -> {response.StatusCode}: {response.ReasonPhrase}");
            throw new Exception($"Error retrieving accounts -> {response.StatusCode}: {response.ReasonPhrase}");
        }

        public async Task<AccountSummaryByYear[]> GetSummaryByYear(Guid id)
        {
            HttpResponseMessage response = await _Client.GetAsync($"/accounts/deposits/summary/year/{id}");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                AccountSummaryByYear[] accounts = JsonConvert.DeserializeObject<AccountSummaryByYear[]>(result);

                return accounts;
            }

            _Logger.LogError($"Error retrieving accounts -> {response.StatusCode}: {response.ReasonPhrase}");
            throw new Exception($"Error retrieving accounts -> {response.StatusCode}: {response.ReasonPhrase}");
        }
    }

}
