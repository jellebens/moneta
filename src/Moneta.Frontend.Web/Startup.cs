using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly.Extensions.Http;
using System.Net.Http;
using Moneta.Frontend.Web.Services;
using Polly;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using OpenTelemetry.Exporter;

namespace Moneta.Frontend.Web
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
            services.AddControllersWithViews();

            services.AddHttpClient<IAccountsService, AccountsService>()
                        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                        .AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<ITransactionsService, TransactionsService>()
                        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                        .AddPolicyHandler(GetRetryPolicy());

            services.AddTransient<IJwtTokenBuilder, JwtTokenBuilder>();


            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(options =>
                {
                    options.ClientId = Configuration["CLIENT_ID"];
                    options.ClientSecret = Configuration["CLIENT_SECRET"];
                    options.Instance = "https://login.microsoftonline.com/";
                    options.Domain = "jellebens.ddns.net";
                    options.TenantId = "consumers";
                    options.CallbackPath = "/auth/microsoft-signin";

                    var redirectToIdpHandler = options.Events.OnRedirectToIdentityProvider;
                    options.Events.OnRedirectToIdentityProvider = async context =>
                    {
                        // Call what Microsoft.Identity.Web is doing
                        await redirectToIdpHandler(context);

                        if (context.ProtocolMessage.RedirectUri.Contains(Configuration["REDIRECT_URI_DOMAIN"]))
                        {
                            context.ProtocolMessage.RedirectUri = context.ProtocolMessage.RedirectUri.Replace("http://", "https://");
                        }
                    };
                });

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddRazorPages()
                 .AddMicrosoftIdentityUI();

            
            services.AddOpenTelemetryTracing((builder) =>
                {
                    builder.AddAspNetCoreInstrumentation();
                    builder.AddHttpClientInstrumentation();
                    builder.AddSource("Moneta.Frontend.Web");
                    builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Configuration["SERVICE"]));
                    builder.AddJaegerExporter(options => {
                        options.AgentHost = Configuration["JAEGER_AGENT_HOST"];
                        options.AgentPort = Convert.ToInt32(Configuration["JAEGER_AGENT_PORT"]);
                        options.ExportProcessorType = ExportProcessorType.Simple;
                    });
                }
            );

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
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.None
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
            });
        }
    }
}
