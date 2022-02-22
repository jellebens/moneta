using AccountService.Events;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moneta.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AccountService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                  .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                  .ConfigureContainer<ContainerBuilder>(builder => {
                        builder.RegisterType<EventDispatcher>().As<IEventDispatcher>();

                        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                               .AsClosedTypesOf(typeof(IEventHandler<>));
                  }).ConfigureLogging(builder =>
                 {
                     builder.ClearProviders();
                     builder.AddProvider(new MonetaLoggerProvider());
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
