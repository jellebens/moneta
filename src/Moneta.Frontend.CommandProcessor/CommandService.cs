using Autofac;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Moneta.Frontend.CommandProcessor.Handlers;
using Moneta.Frontend.Commands;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace Moneta.Frontend.CommandProcessor
{
    public class CommandService : BackgroundService
    {
        private readonly IConfiguration _Configuration;
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<CommandService> _Logger;

        public CommandService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _Logger = loggerFactory.CreateLogger<CommandService>();
            _Configuration = configuration;
            this.loggerFactory = loggerFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _Logger.LogInformation($"Starting service");


            var builder = new ContainerBuilder();

            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsClosedTypesOf(typeof(ICommandHandler<>));

            builder.RegisterInstance(loggerFactory)
                .As<ILoggerFactory>();

            _Logger.LogInformation($"Types registred");
            string connectionString = _Configuration.GetValue<string>("FRONTEND_COMMANDS");

            _Logger.LogInformation($"Creating QueueClient");
            QueueClient queueClient = new QueueClient(connectionString, "frontend-commands");

            queueClient.CreateIfNotExists();

            IContainer container = builder.Build();

            CommandsBinder commandsBinder = new CommandsBinder()
            {
                KnownTypes = typeof(ICommand).Assembly.GetTypes().Where(t => typeof(ICommand).IsAssignableFrom(t)).ToArray()
            };

            _Logger.LogInformation($"Listening for messages");
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await queueClient.ReceiveMessageAsync(cancellationToken: stoppingToken);

                QueueMessage message = response.Value;
                if (message != null)
                {

                    _Logger.LogInformation($"Received message {message.MessageId} body: {message.Body}");
                    try
                    {
                        string msgBody = Encoding.UTF8.GetString(message.Body);
                        dynamic command = JsonConvert.DeserializeObject(msgBody, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All,
                            SerializationBinder = commandsBinder
                        });

                        _Logger.LogInformation($"Command of type: {command.GetType().FullName}");

                        ICommandDispatcher dispatcher = container.Resolve<ICommandDispatcher>();

                        _Logger.LogInformation($"Dispatching Command of type: {command.GetType().FullName}");
                        dispatcher.Dispatch(command);
                    }
                    catch (Exception exc)
                    {
                        _Logger.LogError("Error deserialising command " + exc.Message);
                        throw;
                    }


                    await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, stoppingToken);
                }
            }
            _Logger.LogInformation($"Service Stopped");

        }
    }
}
