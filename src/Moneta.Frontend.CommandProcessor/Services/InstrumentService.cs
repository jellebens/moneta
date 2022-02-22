using Moneta.Frontend.Commands.Instruments;
using Moneta.Frontend.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Services
{
    public interface IInstrumentService: IService
    {
        Task Create(CreateInstrumentCommand newInstrument);
        Task Delete(DeleteInstrumentCommand deleteInstrument);
    }

    public class InstrumentService : ServiceBase, IInstrumentService
    {
        public InstrumentService(IConfiguration configuration, HttpClient client) : base(configuration, client)
        {
            
        }

        public async Task Create(CreateInstrumentCommand newInstrument)
        {
            string json = JsonConvert.SerializeObject(newInstrument);

            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _Client.PostAsync($"/instruments", httpContent);

            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(DeleteInstrumentCommand deleteInstrument)
        {
            HttpResponseMessage response = await _Client.DeleteAsync($"/instruments/{deleteInstrument.InstrumentId}");

            response.EnsureSuccessStatusCode();
        }
    }


}
