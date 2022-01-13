using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moneta.Core;
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
        private static readonly ActivitySource _ActivitySource = new ActivitySource(Telemetry.Source);
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
            _ActivitySource.Dispose();
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

        public Task SendAsync<T>(string queue, string token, T message)
        {
            //FROM: https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
            try
            {
                using (Activity activity = _ActivitySource.StartActivity("Publish message", ActivityKind.Producer))
                {
                    IBasicProperties props = _Channel.CreateBasicProperties();

                    Propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), props, InjectContextIntoHeader);

                    props.Headers.Add("token",token);

                    activity?.SetTag("messaging.system", "rabbitmq");
                    activity?.SetTag("messaging.destination_kind", "queue");
                    activity?.SetTag("messaging.rabbitmq.queue", queue);

                    Dictionary<string, object> args = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", $"{Queues.Frontend.Commands}-deadletter"}
                    };

                    _Channel.QueueDeclare(queue: Queues.Frontend.Commands,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: args);

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
            }
            catch (Exception exc)
            {
                _Logger?.LogError($"Error while sending message {exc.Message}\r\n{exc.StackTrace}");
                throw;
            }
            
            


            return Task.CompletedTask;

        }
    }
}
