using AccountService.Sql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using Microsoft.Identity.Web;

namespace AccountService
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
            services.AddDbContext<AccountsDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetValue<string>("Accounts")
                                   , provideroptions =>
                                   {
                                       provideroptions.EnableRetryOnFailure(5);
                                   });

            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options => { }, options =>
                {
                    options.ClientId = Configuration.GetValue<string>("CLIENT_ID");
                    options.Instance = "https://login.microsoftonline.com/";
                    options.TenantId = "common";
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
