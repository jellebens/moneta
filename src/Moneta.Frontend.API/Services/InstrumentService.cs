using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.API.Models.Instruments;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Services
{
    public interface IInstrumentService: IService
    {
        Task<InstrumentListItem[]> GetAsync();
    }

    public class InstrumentService : ServiceBase, IInstrumentService
    {
        private readonly ILogger<AccountsService> _Logger;

        public InstrumentService(IConfiguration configuration, HttpClient client, ILogger<AccountsService> logger): base(configuration, client)
        {
            _Logger = logger;
        }
        public async Task<InstrumentListItem[]> GetAsync()
        {
            
            using (HttpResponseMessage response = await _Client.GetAsync($"/instruments"))
            {
                //response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                InstrumentListItem[] results = JsonConvert.DeserializeObject<InstrumentListItem[]>(json);

                return results;
            }

        }
    }
}
