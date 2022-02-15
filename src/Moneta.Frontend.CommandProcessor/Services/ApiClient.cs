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
    public interface IApiClient : IService
    {
        Task Complete(Guid id);
        Task Start(Guid id);
    }

    public class ApiClient : ServiceBase, IApiClient
    {
        private readonly ILogger<ApiClient> _Logger;

        public ApiClient(ILogger<ApiClient> logger, IConfiguration configuration, HttpClient client) : base(configuration, client)
        {
            _Logger = logger;
        }



        public async Task Start(Guid id)
        {            
            StringContent httpContent = new StringContent("", Encoding.UTF8, "application/json");
            await _Client.PostAsync($"api/commands/start/{id}", httpContent);
        }

        public async Task Complete(Guid id)
        {
            StringContent httpContent = new StringContent("", Encoding.UTF8, "application/json");
            await _Client.PostAsync($"api/commands/complete/{id}", httpContent);
        }

    }


}
