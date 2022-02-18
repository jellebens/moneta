using Moneta.Frontend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task Execute(string token, TCommand command);
    }

}
