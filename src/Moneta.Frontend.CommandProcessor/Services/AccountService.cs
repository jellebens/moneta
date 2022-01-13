using Moneta.Frontend.Commands;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Services
{
    public interface IAccountsService : IService
    {
        Task Create(CreateAccountCommand newAccount, string jwtToken);

        void Delete(DeleteAccountCommand deleteAccount, string jwtToken);

    }
    public class AccountsService : ServiceBase, IAccountsService
    {

        private readonly ILogger<AccountsService> _Logger;

        public AccountsService(IConfiguration configuration, HttpClient client, ILogger<AccountsService> logger) : base(configuration, client)
        {
            _Logger = logger;

            _Client.BaseAddress = new Uri(configuration["ACCOUNTS_SERVICE"]);
        }

        public async Task Create(CreateAccountCommand newAccount, string jwtToken)
        {
            Authenticate(jwtToken);

            string json = JsonConvert.SerializeObject(newAccount);

            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _Client.PostAsync($"/accounts", httpContent);

            response.EnsureSuccessStatusCode();
        }

        public async void Delete(DeleteAccountCommand deleteAccount, string jwtToken)
        {
            Authenticate(jwtToken);

            HttpResponseMessage response = await _Client.DeleteAsync($"/accounts/{deleteAccount.Id}");

            response.EnsureSuccessStatusCode();
        }

    }
}
