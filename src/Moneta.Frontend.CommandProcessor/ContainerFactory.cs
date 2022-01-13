using Autofac;
using Moneta.Frontend.CommandProcessor.Handlers;
using Moneta.Frontend.CommandProcessor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor
{
    public class ContainerFactory
    {
        public static IContainer Create(ILoggerFactory loggerFactory) {
            var builder = new ContainerBuilder();

            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsClosedTypesOf(typeof(ICommandHandler<>));

            //builder.Register(c => c.Resolve<IHttpClientFactory>().CreateClient());
            //builder.Register(c => new HttpClient()).As<HttpClient>();
            //builder.RegisterType<AccountsService>().As<IAccountsService>();


            //builder.RegisterInstance(loggerFactory)
            //    .As<ILoggerFactory>();

            return builder.Build();

        }
    }
}
