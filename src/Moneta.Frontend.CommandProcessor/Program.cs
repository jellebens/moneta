using Moneta.Core.Logging;
using Moneta.Frontend.CommandProcessor;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using Moneta.Core;
using Polly;
using Polly.Extensions.Http;
using Moneta.Frontend.CommandProcessor.Services;
using Moneta.Core.Jwt;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using System.Reflection;
using Moneta.Frontend.CommandProcessor.Handlers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddProvider(new MonetaLoggerProvider());
    })
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder => {
        builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();

        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .AsClosedTypesOf(typeof(ICommandHandler<>));
    })
    .ConfigureServices((hostContext, services) =>
    {

        services.AddOpenTelemetryTracing((builder) =>
        {
            builder.AddAspNetCoreInstrumentation();
            builder.AddHttpClientInstrumentation();
            builder.AddSource(Telemetry.Source);
            builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(hostContext.Configuration["SERVICE_NAME"]));
            builder.AddJaegerExporter(options =>
            {
                options.AgentHost = hostContext.Configuration["JAEGER_AGENT_HOST"];
                options.AgentPort = Convert.ToInt32(hostContext.Configuration["JAEGER_AGENT_PORT"]);
                options.ExportProcessorType = ExportProcessorType.Simple;
            });
        });

        services.AddHostedService<CommandService>();

        services.AddHttpClient<IAccountsService, AccountsService>(client => {
            client.BaseAddress = new Uri(hostContext.Configuration["ACCOUNTS_SERVICE"]);
        }).SetHandlerLifetime(TimeSpan.FromMinutes(5))
                        .AddPolicyHandler(GetRetryPolicy());

        services.AddTransient<IJwtTokenBuilder, JwtTokenBuilder>();

        
    })
    .Build();

await host.RunAsync();


static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    Random jitterer = new Random();

    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3,    // exponential back-off plus some jitter
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                      + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));
}