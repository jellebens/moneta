using Moneta.Frontend.Commands.Transactions;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Services
{
    public interface ITransactionService : IService
    {
        Task Transfer(NewTransferCommand createTransfer);
    }

    internal class TransactionService : ServiceBase, ITransactionService
    {
        public TransactionService(IConfiguration configuration, HttpClient client) : base(configuration, client)
        {

        }


        public async Task Transfer(NewTransferCommand createTransfer) {
            string json = JsonConvert.SerializeObject(createTransfer);

            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _Client.PostAsync($"/cash/transfer", httpContent);

            response.EnsureSuccessStatusCode();
        }
    }
}
