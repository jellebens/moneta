using Autofac;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Moneta.Frontend.CommandProcessor.Handlers;
using Moneta.Frontend.Commands;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor
{
    public class CommandService : BackgroundService
    {
        private IConfiguration _Configuration;
        private ILogger<CommandService> _Logger;

        public CommandService(IConfiguration configuration, ILogger<CommandService> logger)
        {
            _Logger = logger;
            _Configuration = configuration;
            
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _Logger.LogInformation($"Starting service");


            var builder = new ContainerBuilder();

            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();

            builder.RegisterType<CreateAccountHandler>().As<ICommandHandler<CreateAccountCommand>>();

            builder.RegisterAssemblyTypes(typeof(ICommandHandler<>).GetType().Assembly)
                   .AsClosedTypesOf(typeof(ICommandHandler<>));

            builder.RegisterInstance(new LoggerFactory())
                .As<ILoggerFactory>();

            builder.RegisterGeneric(typeof(Logger<>))
                   .As(typeof(ILogger<>))
                   .SingleInstance();

            _Logger.LogInformation($"Types registred");
            string connectionString = _Configuration.GetValue<string>("FRONTEND_COMMANDS");

            _Logger.LogInformation($"Creating QueueClient");
            QueueClient queueClient = new QueueClient(connectionString, "frontend-commands");

            queueClient.CreateIfNotExists();

            IContainer container = builder.Build();

            CommandsBinder commandsBinder = new CommandsBinder() {
                KnownTypes = new[] { typeof(CreateAccountCommand) }
            };

            var cmd = new CreateAccountCommand { Name = "Test", Currency = "EUR" };

            string json = JsonConvert.SerializeObject(cmd, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                SerializationBinder = commandsBinder
            });
                


            _Logger.LogInformation(json);

            _Logger.LogInformation($"Listening for messages");
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await queueClient.ReceiveMessageAsync(cancellationToken: stoppingToken);

                QueueMessage message = response.Value;
                if (message != null) {


                
                    _Logger.LogInformation($"Received message {message.MessageId} body: {message.Body.ToString()}");
                    try
                    {

                        dynamic command= JsonConvert.DeserializeObject(json, new JsonSerializerSettings
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
