using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Moneta.Frontend.Web.Services
{
    public interface IService {
        void Authenticate(string jwtToken);
    }

    public abstract class ServiceBase : IService
    {
        protected readonly IConfiguration _Configuration;
        protected readonly HttpClient _Client;
        
        protected ServiceBase(IConfiguration configuration, HttpClient client)
        {
            _Configuration = configuration;
            _Client = client;
        }

        public void Authenticate(string jwtToken)
        {
            _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, jwtToken);
        }
    }
}
