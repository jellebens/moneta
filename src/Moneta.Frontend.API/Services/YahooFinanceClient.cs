using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.API.Models.rapidapi;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Services
{
    public interface IYahooFinanceClient  {
        public Task<AutoCompleteResponse> Search(string query);
    }

    public class YahooFinanceClient : IYahooFinanceClient
    {
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _Client;
        private readonly ILogger<AccountsService> _Logger;

        public YahooFinanceClient(IConfiguration configuration, HttpClient client, ILogger<AccountsService> logger)
        {
            _Configuration = configuration;
            _Client = client;
            _Logger = logger;
        }
        public async Task<AutoCompleteResponse> Search(string query)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://yh-finance.p.rapidapi.com/auto-complete?q={query}"),
                Headers = {
                            { "x-rapidapi-host", "yh-finance.p.rapidapi.com" },
                            { "x-rapidapi-key", _Configuration.GetValue<string>("rapidapi-key") },
                },
            };

            using (HttpResponseMessage response = await _Client.SendAsync(request)) { 
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                AutoCompleteResponse autoCompleteResponse = JsonConvert.DeserializeObject<AutoCompleteResponse>(json); 

                return autoCompleteResponse;
            }

        }
    }
}
