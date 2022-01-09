using Autofac;
using Moneta.Frontend.CommandProcessor.Handlers;
using Moneta.Frontend.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor
{
    internal class CommandDispatcher : ICommandDispatcher
    {
        private static readonly ActivitySource Activity = new(nameof(CommandDispatcher));

        private readonly IComponentContext context;

        public CommandDispatcher(IComponentContext context)
        {
            this.context = context;
        }

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = this.context.Resolve<ICommandHandler<TCommand>>();
            //https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
            using (Activity activity = Activity.StartActivity("Dispatching command", ActivityKind.Consumer)) {
                handler.Execute(command);
            }
                
        }
    }
}
