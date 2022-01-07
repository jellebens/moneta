using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.Commands;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Bus
{
    public class RabbitMqBus : IBus, IDisposable
    {
        private readonly CommandsBinder _CommandsBinder = CommandsBinder.Instance();
        private readonly ILogger<RabbitMqBus> _Logger;
        private ConnectionFactory _Factory;
        private IConnection _Connection;
        private IModel _Channel;

        public RabbitMqBus(IConfiguration configuration, ILogger<RabbitMqBus> logger)
        {
            string connectionString = configuration.GetValue<string>("FRONTEND_COMMANDS");

            _Factory = new ConnectionFactory() { HostName = connectionString };
            _Connection = _Factory.CreateConnection();
            _Channel = _Connection.CreateModel();
            _Logger = logger;
        }

        public void Dispose()
        {
            _Logger.LogInformation($"Cleaning up");
            _Channel.Dispose();
            _Connection.Dispose();
        }

        public Task SendAsync<T>(string queue, T message)
        {
            _Channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);



            string json = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                SerializationBinder = _CommandsBinder
            });

            var body = Encoding.UTF8.GetBytes(json);

            _Channel.BasicPublish(exchange: "",
                                 routingKey: queue,
                                 basicProperties: null,
                                 body: body);


            return Task.CompletedTask;

        }
    }
}
