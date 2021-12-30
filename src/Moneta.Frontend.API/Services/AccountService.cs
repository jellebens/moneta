using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Moneta.Frontend.API.Models;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Services
{

    public interface IAccountsService
    {
        Task<AccountListItem[]> GetAsync();
    }


    public class AccountsService : IAccountsService
    {

        private readonly ILogger<AccountsService> _Logger;
        private readonly HttpClient _Client;
        private readonly ITokenAcquisition _TokenAcquisition;

        public AccountsService(IConfiguration configuration, HttpClient client, ITokenAcquisition tokenAcquisition, ILogger<AccountsService> logger)
        {
            _Logger = logger;
            _Client = client;
            _TokenAcquisition = tokenAcquisition;
        }

        public async Task<AccountListItem[]> GetAsync()
        {

            string[] scopes = new string[] { "api://3652d22c-6197-44a5-9334-da5a8c45182d/access_as_user" };
            string accessToken = await _TokenAcquisition.GetAccessTokenForUserAsync(scopes);

            _Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

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
    }

}
