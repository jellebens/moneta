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
using Moneta.Core.Jwt;
using Moneta.Core;

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

            services.AddTransient<IJwtTokenBuilder, JwtTokenBuilder>();

            services.AddHeaderPropagation(o =>
            {
                // propagate the header if present
                o.Headers.Add("x-request-id");
                o.Headers.Add("x-b3-traceid");
                o.Headers.Add("x-b3-spanid");
                o.Headers.Add("x-b3-parentspanid");
                o.Headers.Add("x-b3-sampled");
                o.Headers.Add("x-b3-flags");
                o.Headers.Add("x-ot-span-context");
            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                        .AddMicrosoftIdentityWebApi(options => { }, options =>
                                        {
                                            options.ClientId = Configuration.GetValue<string>("CLIENT_ID");
                                            options.Instance = "https://login.microsoftonline.com/";
                                            options.TenantId = "common";
                                            options.ClientSecret = Configuration.GetValue<string>("CLIENT_SECRET");
                                        });
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(jwt =>
            //{
            //    byte[] key = Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWT_SECRET"));

            //    jwt.SaveToken = true;
            //    jwt.Audience = Configuration.GetValue<string>("CLIENT_ID");
            //    jwt.Authority = "https://login.microsoftonline.com/common";
            //    jwt.RequireHttpsMetadata = false;

            //    jwt.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = true,
            //        ValidIssuer = "https://login.microsoftonline.com/common",
            //        ValidateAudience = true,
            //        RequireExpirationTime = false,
            //        ValidateLifetime = true
            //    };
            //});

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpLogging();

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
