﻿using System;
using Moneta.Core;
using Moneta.Frontend.Commands;
using Newtonsoft.Json;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;

using System.Text;

namespace Moneta.Frontend.CommandProcessor
{
    public class CommandService : IHostedService
    {
        private readonly ICommandDispatcher _Dispatcher;
        private readonly IConfiguration _Configuration;
        private readonly ILogger<CommandService> _Logger;
        private readonly ILoggerFactory _LoggerFactory;

        private IConnection _Connection;
        private IModel _Channel;
        private EventingBasicConsumer consumer;

        private HubConnection _HubConnection;

        private static readonly ActivitySource Activity = new(Telemetry.Source);
        private static readonly TextMapPropagator Propagator = new TraceContextPropagator();

        public CommandService(ICommandDispatcher dispatcher, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _Logger = loggerFactory.CreateLogger<CommandService>();
            _LoggerFactory = loggerFactory;
            _Dispatcher = dispatcher;
            
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


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _Logger.LogInformation($"Starting service");

            string connectionString = _Configuration.GetValue<string>("RABBITMQ_HOST");

            //IContainer container = ContainerFactory.Create(_LoggerFactory);

            _Logger.LogInformation($"Creating Client");

            var factory = new ConnectionFactory() { HostName = connectionString };
            _Connection = factory.CreateConnection();
            _Channel = _Connection.CreateModel();

            Dictionary<string, object> args = new Dictionary<string, object>
                    {
                        {"x-dead-letter-exchange", $"{Queues.Frontend.Commands}-deadletter"}
                    };

            _Channel.QueueDeclare(queue: Queues.Frontend.Commands,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: args);

            consumer = new EventingBasicConsumer(_Channel);

            _HubConnection = new HubConnectionBuilder()
                .WithUrl(_Configuration.GetValue<string>("COMMANDS_HUB"))
                                    .WithAutomaticReconnect()
                                    .Build() ;
            await _HubConnection.StartAsync();
            consumer.Received += async (model, ea) =>
            {
                IBasicProperties properties = ea.BasicProperties;
                var parentContext = Propagator.Extract(default, properties, ExtractTraceContextFromBasicProperties);
                
                Baggage.Current = parentContext.Baggage;

                using (var activity = Activity.StartActivity("Process Message", ActivityKind.Consumer, parentContext.ActivityContext))
                {

                    string token = Encoding.UTF8.GetString(properties.Headers["token"] as byte[]);
                    byte[] body = ea.Body.ToArray();
                    string message = Encoding.UTF8.GetString(body);



                    //Add Tags to the Activity
                    activity?.SetTag("messaging.system", "rabbitmq");
                    activity?.SetTag("messaging.destination_kind", "queue");
                    activity?.SetTag("messaging.rabbitmq.queue", Queues.Frontend.Commands);

                    dynamic command = JsonConvert.DeserializeObject(message, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        SerializationBinder = CommandsBinder.Instance()
                    });

                    Guid id = ((ICommand)command).Id;
                    try
                    {

                        CommandStatus start = CommandStatus.Start(command.Id);
                        await _HubConnection.SendAsync("Update", id , start);
                        _Dispatcher.Dispatch(token, command);
                        Thread.Sleep(10000);
                        _Channel.BasicAck(ea.DeliveryTag, false);
                        CommandStatus complete = CommandStatus.Complete(command.Id);
                        await _HubConnection.SendAsync("Update", id, complete);
                    }
                    catch (Exception exc)
                    {
                        _Logger.LogError($"Exception occured when executing command ({exc.Message}), deadlettering message.");
                        _Channel.BasicNack(ea.DeliveryTag, false, false);
                        CommandStatus complete = CommandStatus.Complete(command.Id, exc.Message);
                        await _HubConnection.SendAsync("Update", id, complete);
                    }



                }
            };

            _Channel.BasicConsume(Queues.Frontend.Commands, false, consumer);

            _Logger.LogInformation($"Listening for messages");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _Logger.LogInformation($"Shutting down service");

            _Channel.Dispose();
            
            _Connection.Dispose();

            await _HubConnection.DisposeAsync();
        }
    }
}
