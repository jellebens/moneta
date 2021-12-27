using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using Microsoft.IdentityModel.Tokens;

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options => { }, options => {
                    options.ClientId = Configuration.GetValue<string>("CLIENT_ID");
                    options.Instance = "https://login.microsoftonline.com/";
                    options.TenantId = "common";
                });
            //.AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));



            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Moneta.Frontend.API", Version = "v1" });
            });

            services.AddOpenTelemetryTracing((builder) =>
            {
                builder.AddAspNetCoreInstrumentation();
                builder.AddHttpClientInstrumentation();
                builder.AddSource("moneta");
                builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Configuration["SERVICE_NAME"]));
                builder.AddJaegerExporter(options =>
                {
                    options.AgentHost = Configuration["JAEGER_AGENT_HOST"];
                    options.AgentPort = Convert.ToInt32(Configuration["JAEGER_AGENT_PORT"]);
                    options.ExportProcessorType = ExportProcessorType.Simple;
                });
            });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
