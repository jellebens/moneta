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
        Task Complete(Guid id, string jwtToken);
        Task Start(Guid id, string jwtToken);
    }

    public class ApiClient : ServiceBase, IApiClient
    {
        private readonly ILogger<ApiClient> _Logger;

        public ApiClient(ILogger<ApiClient> logger, IConfiguration configuration, HttpClient client) : base(configuration, client)
        {
            _Client.BaseAddress = new Uri(configuration["API_SERVICE"]);
            _Logger = logger;
        }

        public async Task Start(Guid id, string jwtToken)
        {
            Authenticate(jwtToken);

            CommandStatus status = CommandStatus.Start(id);

            string json = JsonConvert.SerializeObject(status);

            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _Client.PutAsync($"api/commands/status", httpContent);

            response.EnsureSuccessStatusCode();
        }

        public async Task Complete(Guid id, string jwtToken)
        {
            Authenticate(jwtToken);

            CommandStatus status = CommandStatus.Complete(id);

            string json = JsonConvert.SerializeObject(status);

            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _Client.PutAsync($"api/commands/status", httpContent);

            response.EnsureSuccessStatusCode();
        }

    }


}
