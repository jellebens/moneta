using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Moneta.Frontend.Web.Models;
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

    public interface IAccountsService: IService
    {
        Task<AccountInfo[]> ListAsync();
        
        Task<HttpResponseMessage> CreateAccountAsync(NewAccountModel model);
        Task DeleteAsync(Guid id);
    }
    public class AccountsService : ServiceBase, IAccountsService
    {
        
        private readonly ILogger<AccountsService> _Logger;

        public AccountsService(IConfiguration configuration, HttpClient client, ILogger<AccountsService> logger): base(configuration, client)
        {
            _Logger = logger;
            
            _Client.BaseAddress = new Uri(configuration["ACCOUNTS_SERVICE"]);
        }

        public async Task<HttpResponseMessage> CreateAccountAsync(NewAccountModel model) {
            var createAccountCommand = new
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Currency = model.Currency
            };

            StringContent data = new StringContent(JsonConvert.SerializeObject(createAccountCommand), Encoding.UTF8, "application/json");

            return await _Client.PostAsync("/accounts", data);
        }

        public async Task DeleteAsync(Guid id)
        {

            HttpResponseMessage response = await _Client.DeleteAsync($"/accounts/{id}");

            response.EnsureSuccessStatusCode();
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
