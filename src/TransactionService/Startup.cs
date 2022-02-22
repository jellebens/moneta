using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Text;
using TransactionService.Services;
using TransactionService.Sql;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using Microsoft.Identity.Web;
using TransactionService.Bus;
using Moneta.Core;

namespace TransactionService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TransactionsDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetValue<string>("Transactions")
                                   , provideroptions =>
                                   {
                                       provideroptions.EnableRetryOnFailure(5);
                                   });

            });

            services.AddScoped<IBus, RabbitMqBus>();

            services.AddHttpClient<IAccountsService, AccountsService>(client => {
                client.BaseAddress = new Uri(Configuration["ACCOUNTS_SERVICE"]);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5))
                        .AddPolicyHandler(GetRetryPolicy());

            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                        .AddMicrosoftIdentityWebApi(options => { }, options =>
                                        {
                                            options.ClientId = Configuration.GetValue<string>("CLIENT_ID");
                                            options.Instance = "https://login.microsoftonline.com/";
                                            options.TenantId = "common";
                                            options.ClientSecret = Configuration.GetValue<string>("CLIENT_SECRET");
                                        });

            services.AddControllers();

            services.AddOpenTelemetryTracing((builder) =>
            {
                builder.AddAspNetCoreInstrumentation();
                builder.AddSqlClientInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.SetDbStatementForText = true;
                    options.SetDbStatementForStoredProcedure = true;
                });
                builder.AddHttpClientInstrumentation();
                builder.AddSource(Telemetry.Source);
                builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Configuration["SERVICE_NAME"]));
                builder.AddJaegerExporter(options =>
                {
                    options.AgentHost = Configuration["JAEGER_AGENT_HOST"];
                    options.AgentPort = Convert.ToInt32(Configuration["JAEGER_AGENT_PORT"]);
                    options.ExportProcessorType = ExportProcessorType.Simple;
                });
            }
            );

            services.AddTransient<IStartupFilter, DatabaseUpgradeFilter>();
        }

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
