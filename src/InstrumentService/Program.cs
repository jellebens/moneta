using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using Polly;
using Polly.Extensions.Http;
using Moneta.Core;
using InstrumentService.Sql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<InstrumentsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("Instruments")
                       , provideroptions =>
                       {
                           provideroptions.EnableRetryOnFailure(5);
                       });

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOpenTelemetryTracing((b) =>
     {
         b.AddAspNetCoreInstrumentation();
         b.AddHttpClientInstrumentation();
         b.AddSource(Telemetry.Source);
         b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Configuration["SERVICE_NAME"]));
         b.AddJaegerExporter(options =>
         {
             options.AgentHost = builder.Configuration["JAEGER_AGENT_HOST"];
             options.AgentPort = Convert.ToInt32(builder.Configuration["JAEGER_AGENT_PORT"]);
             options.ExportProcessorType = ExportProcessorType.Simple;
         });
     });

builder.Services.AddTransient<IStartupFilter, DatabaseUpgradeFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


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