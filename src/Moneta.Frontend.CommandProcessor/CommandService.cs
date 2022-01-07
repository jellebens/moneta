﻿using Autofac;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Moneta.Frontend.CommandProcessor.Handlers;
using Moneta.Frontend.Commands;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

        private IConnection connection;
        private IModel channel;

        public CommandService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _Logger = loggerFactory.CreateLogger<CommandService>();
            _LoggerFactory = loggerFactory;
            _Configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _Logger.LogInformation($"Starting service");

            string connectionString = _Configuration.GetValue<string>("FRONTEND_COMMANDS");

            IContainer container = ContainerFactory.Create(_LoggerFactory);

            _Logger.LogInformation($"Creating Client");

            CommandsBinder commandsBinder = new CommandsBinder()
            {
                KnownTypes = Known.Types()
            };

            _Logger.LogInformation($"Listening for messages");

            var factory = new ConnectionFactory() { HostName = connectionString };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: "frontend-commands",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                dynamic command = JsonConvert.DeserializeObject(message, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    SerializationBinder = commandsBinder
                });

                ICommandDispatcher dispatcher = container.Resolve<ICommandDispatcher>();

                _Logger.LogInformation($"Dispatching Command of type: {command.GetType().FullName}");
                dispatcher.Dispatch(command);
            };


            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            channel.Dispose();
            connection.Dispose();


            _Logger.LogInformation($"Shutting down service");
            return Task.CompletedTask;

        }
    }
}
