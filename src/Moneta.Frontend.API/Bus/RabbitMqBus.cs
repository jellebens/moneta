using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Frontend.Commands;
using Newtonsoft.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static readonly ActivitySource Activity = new(nameof(RabbitMqBus));
        private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;


        public RabbitMqBus(IConfiguration configuration, ILogger<RabbitMqBus> logger)
        {
            string connectionString = configuration.GetValue<string>("RABBITMQ_HOST");

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

        private void InjectContextIntoHeader(IBasicProperties props, string key, string value)
        {
            try
            {
                props.Headers ??= new Dictionary<string, object>();
                props.Headers[key] = value;
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Failed to inject trace context.");
            }
        }

        public Task SendAsync<T>(string queue, T message)
        {
            //FROM: https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
            using (Activity activity = Activity.StartActivity("Publish message"))
            {

                IBasicProperties props = _Channel.CreateBasicProperties();

                Propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), props, InjectContextIntoHeader);
                activity?.SetTag("messaging.system", "rabbitmq");
                activity?.SetTag("messaging.destination_kind", "queue");
                activity?.SetTag("messaging.rabbitmq.queue", queue);


                _Channel.QueueDeclare(queue: queue,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);



                string json = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    SerializationBinder = _CommandsBinder
                });

                var body = Encoding.UTF8.GetBytes(json);

                _Channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: props,
                                     body: body); 
            }


            return Task.CompletedTask;

        }
    }
}
