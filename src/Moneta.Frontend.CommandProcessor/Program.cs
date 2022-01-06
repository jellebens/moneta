using Moneta.Core.Logging;
using Moneta.Frontend.CommandProcessor;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddProvider(new MonetaLoggerProvider());
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<CommandService>();
    })
    .Build();

await host.RunAsync();


