using Moneta.Frontend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.CommandProcessor
{
    public interface ICommandDispatcher
    {
        void Dispatch<TCommand>(string token, TCommand command) where TCommand : ICommand;
    }
}
