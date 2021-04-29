using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Moneta.Frontend.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Services
{
    public interface ITransactionsService: IService
    {
       
        Task<HttpResponseMessage> CreateTransactionAsync(TransactionHeaderModel model);
        Task<TransactionAmountModel> GetAmount(Guid id);
        Task<HttpResponseMessage> UpdateAmountAsync(Guid transactionId, TransactionAmountModel model);
        Task<HttpResponseMessage> UpdateCostsAsync(Guid transactionId, TransactionCostsModel model);
        Task<TransactionAmountOverviewModel> AmountOverviewAsync(Guid id);
        Task<TransactionCostsOverviewModel> CostOverviewAsync(Guid id);
        Task<TransactionHeaderOverviewModel> TransactionOverview(Guid id);
    }

    public class TransactionsService: ServiceBase, ITransactionsService
    {
        public TransactionsService(IConfiguration configuration, HttpClient client): base(configuration, client)
        {
            _Client.BaseAddress = new Uri(_Configuration["TRANSACTIONS_SERVICE"]);
        }

        public Task<TransactionAmountModel> GetAmount(Guid id)
        {

            return Task.Run(() =>
            {
                return new TransactionAmountModel();
            });
            
        }

        public async Task<HttpResponseMessage> CreateTransactionAsync(TransactionHeaderModel model) {

            var startTransactionCommand = new
            {
                Id = model.Id,
                AccountId = model.SelectedAccount,
                TransactionNumber = model.TransactionNumber,
                Symbol = model.Symbol,
                TransactionDate = DateTime.ParseExact(model.TransactionDate, "dd/MM/yyyy", null),
                Currency = model.Currency
            };

            StringContent data = new StringContent(JsonConvert.SerializeObject(startTransactionCommand), Encoding.UTF8, "application/json");

            return await _Client.PostAsync("/buyorders/new", data);
        }

        public async Task<HttpResponseMessage> UpdateAmountAsync(Guid transactionId, TransactionAmountModel model)
        {

            var updateAmountCommand = new
            {
                Price = model.Price,
                Quantity = model.Quantity,
                Exchangerate = model.Exchangerate
            };

            StringContent data = new StringContent(JsonConvert.SerializeObject(updateAmountCommand), Encoding.UTF8, "application/json");

            return await _Client.PutAsync($"/buyorders/{transactionId}/amount", data);
        }

        public async Task<HttpResponseMessage> UpdateCostsAsync(Guid transactionId, TransactionCostsModel model)
        {

            var updateCostsCommand = new
            {
                Id = Guid.NewGuid(),
                Commision = model.Commission,
                CostExchangerate = model.CostExchangeRate,
                StockMarketTax = model.StockMarketTax
            };

            StringContent data = new StringContent(JsonConvert.SerializeObject(updateCostsCommand), Encoding.UTF8, "application/json");

            return await _Client.PutAsync($"/buyorders/{transactionId}/costs", data);
        }

        public async Task<TransactionAmountOverviewModel> AmountOverviewAsync(Guid transactionId)
        {
            HttpResponseMessage response = await _Client.GetAsync($"/buyorders/{transactionId}/amount");

            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            TransactionAmountOverviewModel model = JsonConvert.DeserializeObject<TransactionAmountOverviewModel>(result);

            return model;
        }

        public async Task<TransactionCostsOverviewModel> CostOverviewAsync(Guid transactionId)
        {
            HttpResponseMessage response = await _Client.GetAsync($"/buyorders/{transactionId}/costs");

            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            TransactionCostsOverviewModel model = JsonConvert.DeserializeObject<TransactionCostsOverviewModel>(result);

            return model;
        }

        public async Task<TransactionHeaderOverviewModel> TransactionOverview(Guid transactionId)
        {
            HttpResponseMessage response = await _Client.GetAsync($"/buyorders/{transactionId}");

            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            TransactionHeaderOverviewModel model = JsonConvert.DeserializeObject<TransactionHeaderOverviewModel>(result);

            return model;
        }
    }
}
