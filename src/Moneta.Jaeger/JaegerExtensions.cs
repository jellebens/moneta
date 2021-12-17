using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTracing;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jaeger
{
    public class JaegerOptions
    {
        public string ServiceName { get; set; }
        public string Host { get; set; }

        public int? Port { get; set; }

        public double SamplingRate { get; set; }
        public double LowerBound { get; set; }

        public JaegerOptions()
        {
            SamplingRate = 0.1d;
            LowerBound = 1d;
            Host = "localhost";
            Port = 6831;
        }



    }
    public static class JaegerExtensions
    {
        public static IServiceCollection AddJaegerTracing(this IServiceCollection services, Action<JaegerOptions> setupAction = null)
        {

            if (setupAction != null) services.ConfigureJaegerTracing(setupAction);

            services.AddSingleton<ITracer>(cli =>
            {
                ILoggerFactory loggerFactory = cli.GetRequiredService<ILoggerFactory>();

                ILogger logger = loggerFactory.CreateLogger("Jaeger");

                JaegerOptions options = cli.GetService<IOptions<JaegerOptions>>().Value;

                Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
                                            .RegisterSenderFactory<ThriftSenderFactory>();

                var senderConfig = new Jaeger.Configuration.SenderConfiguration(loggerFactory)
                    .WithAgentHost(options.Host)
                    .WithAgentPort(options.Port);

                var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(senderConfig.GetSender())
                    .Build();

                logger.LogInformation($"Jaeger sending to {senderConfig.GetSender()}");

                var sampler = new GuaranteedThroughputSampler(options.SamplingRate, options.LowerBound);

                var tracer = new Tracer.Builder(options.ServiceName ?? "Not Set")
                    .WithLoggerFactory(loggerFactory)
                    .WithReporter(reporter)
                    .WithSampler(sampler)
                    .Build();

                // Allows code that can't use dependency injection to have access to the tracer.
                if (!GlobalTracer.IsRegistered())
                {
                    GlobalTracer.Register(tracer);
                }

                return tracer;
            });

            services.AddOpenTracing(builder =>
            {
                builder.ConfigureAspNetCore(options =>
                {
                    options.Hosting.IgnorePatterns.Add(x =>
                    {
                        return x.Request.Path.Value.EndsWith("live", StringComparison.InvariantCultureIgnoreCase);
                    });
                    options.Hosting.IgnorePatterns.Add(x =>
                    {
                        return x.Request.Path.ToString().EndsWith("ready", StringComparison.InvariantCultureIgnoreCase);
                    });
                    options.Hosting.IgnorePatterns.Add(x =>
                    {
                        return x.Request.Path == "/metrics";
                    });
                });
            });

            return services;
        }

        public static void ConfigureJaegerTracing(this IServiceCollection services, Action<JaegerOptions> setupAction)
        {
            services.Configure<JaegerOptions>(setupAction);
        }
    }
}