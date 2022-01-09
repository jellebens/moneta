using Autofac;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Moneta.Core;
using Moneta.Frontend.CommandProcessor.Handlers;
using Moneta.Frontend.Commands;
using Newtonsoft.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Moneta.Frontend.CommandProcessor
{
    public class CommandService : IHostedService
    {
        private readonly IConfiguration _Configuration;
        private readonly ILogger<CommandService> _Logger;
        private readonly IContainer container;
        private readonly ILoggerFactory _LoggerFactory;

        private IConnection _Connection;
        private IModel _Channel;

        private static readonly ActivitySource Activity = new(nameof(CommandService));
        private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

        public CommandService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _Logger = loggerFactory.CreateLogger<CommandService>();
            _LoggerFactory = loggerFactory;
            _Configuration = configuration;
        }

        //Extract the Activity from the message header
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

       

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _Logger.LogInformation($"Starting service");

            string connectionString = _Configuration.GetValue<string>("RABBITMQ_HOST");

            IContainer container = ContainerFactory.Create(_LoggerFactory);

            _Logger.LogInformation($"Creating Client");

            var factory = new ConnectionFactory() { HostName = connectionString };
            _Connection = factory.CreateConnection();
            _Channel = _Connection.CreateModel();

            _Channel.QueueDeclare(queue: Queues.Frontend.Commands,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_Channel);

            consumer.Received += (model, ea) =>
            {
                var parentContext = Propagator.Extract(default, ea.BasicProperties, ExtractTraceContextFromBasicProperties);
                Baggage.Current = parentContext.Baggage;

                using (var activity = Activity.StartActivity("Process Message", ActivityKind.Consumer, parentContext.ActivityContext))
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    //Add Tags to the Activity
                    activity?.SetTag("messaging.system", "rabbitmq");
                    activity?.SetTag("messaging.destination_kind", "queue");
                    activity?.SetTag("messaging.rabbitmq.queue", Queues.Frontend.Commands);

                    dynamic command = JsonConvert.DeserializeObject(message, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        SerializationBinder = CommandsBinder.Instance()
                    });

                    ICommandDispatcher dispatcher = container.Resolve<ICommandDispatcher>();

                    _Logger.LogInformation($"Dispatching Command of type: {command.GetType().FullName}");
                    dispatcher.Dispatch(command);
                }
            };
            
            _Channel.BasicConsume(Queues.Frontend.Commands, true, consumer);

            _Logger.LogInformation($"Listening for messages");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _Channel.Dispose();
            _Connection.Dispose();


            _Logger.LogInformation($"Shutting down service");
            return Task.CompletedTask;

        }
    }
}
