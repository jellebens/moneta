using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Moneta.Core.Jwt;
using Moneta.Frontend.API.Models;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Services
{

    public interface IAccountsService
    {
        Task<AccountListItem[]> GetAsync();
        void Authenticate(string token);
    }


    public class AccountsService : IAccountsService
    {

        private readonly ILogger<AccountsService> _Logger;
        private readonly HttpClient _Client;
        
        public AccountsService(IConfiguration configuration, HttpClient client, ILogger<AccountsService> logger)
        {
            _Logger = logger;
            _Client = client;
        }

        public void Authenticate(string token)
        {
            _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<AccountListItem[]> GetAsync(string token)
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

        public Task<AccountListItem[]> GetAsync()
        {
            throw new NotImplementedException();
        }
    }

}
