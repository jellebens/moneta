using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moneta.Core;
using Moneta.Events;
using Newtonsoft.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccountService.Events
{
    public class EventHandlerService : IHostedService
    {
        private readonly IEventDispatcher _Dispatcher;
        private readonly IConfiguration _Configuration;
        private readonly ILogger<EventHandlerService> _Logger;

        private IConnection _Connection;
        private IModel _Channel;

        private static readonly ActivitySource Activity = new(Telemetry.Source);
        private static readonly TextMapPropagator Propagator = new TraceContextPropagator();

        public EventHandlerService(IEventDispatcher dispatcher, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _Logger = loggerFactory.CreateLogger<EventHandlerService>();
            _Dispatcher = dispatcher;
            _Configuration = configuration;

        }

        private IEnumerable<string> ExtractTraceContextFromBasicProperties(IBasicProperties props, string key)
        {
            try
            {
                if (props.Headers.TryGetValue(key, out var value))
                {
                    var bytes = value as byte[];
                    return new[] { Encoding.UTF8.GetString(bytes) };
                }
            }
            catch (Exception ex)
            {
                _Logger.LogError($"Failed to extract trace context: {ex}");
            }

            return Enumerable.Empty<string>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _Logger.LogInformation($"Starting eventhandler service");

            string connectionString = _Configuration.GetValue<string>("RABBITMQ_HOST");

            _Logger.LogInformation($"Creating Client");

            var factory = new ConnectionFactory() { HostName = connectionString };
            _Connection = factory.CreateConnection();
            _Channel = _Connection.CreateModel();

            Dictionary<string, object> args = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", $"{Topics.Transactions}-deadletter"}
                    };

            _Channel.ExchangeDeclare(exchange: Topics.Transactions, type: ExchangeType.Fanout, arguments: args);

            var queueName = _Channel.QueueDeclare().QueueName;

            _Channel.QueueBind(queue: queueName,
                              exchange: Topics.Transactions,
                              routingKey: "");


            EventingBasicConsumer consumer = new EventingBasicConsumer(_Channel);

            consumer.Received += (model, ea) => {
                IBasicProperties properties = ea.BasicProperties;
                var parentContext = Propagator.Extract(default, properties, ExtractTraceContextFromBasicProperties);

                Baggage.Current = parentContext.Baggage;

                using (var activity = Activity.StartActivity("Process event", ActivityKind.Consumer, parentContext.ActivityContext))
                {
                    //Add Tags to the Activity
                    activity?.SetTag("messaging.system", "rabbitmq");
                    activity?.SetTag("messaging.destination_kind", "topic");
                    activity?.SetTag("messaging.destination", Topics.Transactions);

                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    dynamic evnt = JsonConvert.DeserializeObject(message, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        SerializationBinder = EventsBinder.Instance()
                    });

                    _Dispatcher.Dispatch(evnt);
                }

            };

            _Channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _Logger.LogInformation($"Stopping eventhandler service");
            _Channel.Dispose();
            _Connection.Dispose();
        }
    }
}
