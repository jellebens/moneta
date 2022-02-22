using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using Moneta.Frontend.API.Services;
using Polly.Extensions.Http;
using Polly;
using System.Net.Http;
using Moneta.Core.Jwt;
using Moneta.Frontend.API.Bus;
using System.Diagnostics;
using Moneta.Core;
using Moneta.Frontend.API.Hubs;
using Microsoft.AspNetCore.Authentication;

namespace Moneta.Frontend.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //https://github.com/Azure-Samples/ms-identity-javascript-react-spa-dotnetcore-webapi-obo/
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddScoped<IBus, RabbitMqBus>();


            //https://damienbod.com/2020/11/09/implement-a-web-app-and-an-asp-net-core-secure-api-using-azure-ad-which-delegates-to-second-api/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options => { }, options =>
                {
                    options.ClientId = Configuration.GetValue<string>("CLIENT_ID");
                    options.Instance = "https://login.microsoftonline.com/";
                    options.TenantId = "common";
                });

            

            services.AddControllers();
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Moneta.Frontend.API", Version = "v1" });
            });

            services.AddHttpClient<IAccountsService, AccountsService>(client => {
                                                                                    client.BaseAddress = new Uri(Configuration["ACCOUNTS_SERVICE"]);
                                                                                }).SetHandlerLifetime(TimeSpan.FromMinutes(5))  
                                                                                  .AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<IYahooFinanceClient, YahooFinanceClient>().SetHandlerLifetime(TimeSpan.FromMinutes(5))
                                                                       .AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<IInstrumentService, InstrumentService>(client => {
                client.BaseAddress = new Uri(Configuration["INSTRUMENTS_SERVICE"]);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5))
                                                                                  .AddPolicyHandler(GetRetryPolicy());

            services.AddOpenTelemetryTracing((builder) =>
            {
                builder.AddAspNetCoreInstrumentation((options) => options.Filter = httpContext =>
                    {
                        return !httpContext.Request.Path.StartsWithSegments("/api/sys", StringComparison.OrdinalIgnoreCase);
                    }
                );
                builder.AddHttpClientInstrumentation();
                builder.AddSource(Telemetry.Source);
                builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Configuration["SERVICE_NAME"]));
                builder.AddJaegerExporter(options =>
                {
                    options.AgentHost = Configuration["JAEGER_AGENT_HOST"];
                    options.AgentPort = Convert.ToInt32(Configuration["JAEGER_AGENT_PORT"]);
                    options.ExportProcessorType = ExportProcessorType.Simple;
                });
                //builder.AddConsoleExporter();
            }
           );
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
                                       .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                                       .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                    retryAttempt)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moneta.Frontend.API v1"));
            }

            //app.UseHttpsRedirection();



            app.UseCors(builder =>
               builder.WithOrigins("http://localhost:3000")
                      .WithOrigins("https://jellebens.ddns.net")
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .AllowAnyMethod());

            app.UseRouting();
            
            app.UseMiddleware<WebSocketsMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapHub<CommandHub>("/hubs/commands");
            });
        }
    }
}
