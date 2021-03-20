using Microsoft.Extensions.Configuration;
using Moneta.Frontend.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Clients
{
    public interface ITransactionsService
    {
        Task<HttpResponseMessage> CreateAsync(TransactionModel model);
     }

    public class TransactionsService: ITransactionsService
    {
        private readonly IConfiguration _Configuration;
        private readonly HttpClient _Client;

        public TransactionsService(IConfiguration configuration, HttpClient client)
        {
            this._Configuration = configuration;
            this._Client = client;
            _Client.BaseAddress = new Uri(_Configuration["TRANSACTIONS_SERVICE"]);
        }

        public async Task<HttpResponseMessage> CreateAsync(TransactionModel model) {
            
            var importTransactionCommand = new
            {
                Id = Guid.NewGuid(),
                TransactionNumber = model.TransactionNumber,
                Symbol = model.Symbol,
                TransactionDate = DateTime.ParseExact(model.TransactionDate, "dd/MM/yyyy", null),
                Price = model.Price,
                Currency = model.Currency,
                Quantity = model.Quantity,
                Subtotal = model.Subtotal,
                ExchangeRate = model.ExchangeRate,
                Commission = model.Commission ?? 0,
                ExchangeRateFee = model.ExchangeRateFee ?? 0,
                TOB = model.TOB ?? 0,
                TotalCosts = model.TotalCosts,
                Total = model.Total,
                SelectedAccount = model.SelectedAccount
            };


            StringContent data = new StringContent(JsonConvert.SerializeObject(importTransactionCommand), Encoding.UTF8, "application/json");

            return await _Client.PostAsync("/buyorder/create", data);
        }
    }
}
