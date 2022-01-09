using Moneta.Core.Logging;
using Moneta.Frontend.CommandProcessor;
using Autofac.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddProvider(new MonetaLoggerProvider());
    })
    .ConfigureServices((hostContext, services) =>
    {

        services.AddOpenTelemetryTracing((builder) =>
        {
            builder.AddAspNetCoreInstrumentation();
            builder.AddHttpClientInstrumentation();
            builder.AddSource("moneta");
            builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(hostContext.Configuration["SERVICE_NAME"]));
            builder.AddJaegerExporter(options =>
            {
                options.AgentHost = hostContext.Configuration["JAEGER_AGENT_HOST"];
                options.AgentPort = Convert.ToInt32(hostContext.Configuration["JAEGER_AGENT_PORT"]);
                options.ExportProcessorType = ExportProcessorType.Simple;
            });
        });

        services.AddHostedService<CommandService>();
    })
    .Build();

await host.RunAsync();


