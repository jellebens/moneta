using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.API.Models.Yfapi;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Services
{
    public interface IYahooFinanceClient
    {
        public Task<QuoteResults> Detail(string symbol);
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
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://yfapi.net/v6/finance/autocomplete?lang=en&query={query}"),
                Headers ={
                            { "x-api-key", _Configuration.GetValue<string>("financeapi-key") },
                         },
            };


            using (HttpResponseMessage response = await _Client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                AutoCompleteResponse autoCompleteResponse = JsonConvert.DeserializeObject<AutoCompleteResponse>(json);

                return autoCompleteResponse;
            }

        }

        public async Task<QuoteResults> Detail(string symbol)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://yfapi.net/v6/finance/quote?region=US&lang=en&symbols={symbol}"),
                Headers ={
                            { "x-api-key", _Configuration.GetValue<string>("financeapi-key") },
                         },
            };


            using (HttpResponseMessage response = await _Client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                QuoteResults results = JsonConvert.DeserializeObject<QuoteResults>(json);

                return results;
            }

        }
    }
}
