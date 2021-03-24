using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moneta.Frontend.Web.Services;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;


namespace Moneta.UI
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
                        .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
                        .AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<ITransactionsService, TransactionsService>()
                        .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
                        .AddPolicyHandler(GetRetryPolicy());

            
            services.AddAuthentication(x => {
                x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/auth/login"; // Must be lowercase
            }).AddFacebook(facebookOptions => {
                                                 facebookOptions.AppId = Configuration["FACEBOOK_APPID"];
                                                 facebookOptions.AppSecret = Configuration["FACEBOOK_APPSECRET"];
             });
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
