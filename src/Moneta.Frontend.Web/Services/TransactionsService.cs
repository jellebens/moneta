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
       
        Task<HttpResponseMessage> StartAsync(TransactionHeaderModel model);
        Task<TransactionAmountModel> GetAmount(Guid id);
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

        public async Task<HttpResponseMessage> StartAsync(TransactionHeaderModel model) {

            var startTransactionCommand = new
            {
                Id = model.Id,
                AccountId = model.SelectedAccount,
                TransactionNumber = model.TransactionNumber,
                Symbol = model.Symbol,
                TransactionDate = DateTime.ParseExact(model.TransactionDate, "dd/MM/yyyy", null)
            };

            StringContent data = new StringContent(JsonConvert.SerializeObject(startTransactionCommand), Encoding.UTF8, "application/json");

            return await _Client.PostAsync("/buyorder/new", data);
        }
    }
}
