using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneta.Frontend.Commands
{
    public class CommandStatus
    {
        public Guid CommandId { get; set; }

        public string Status { get; set; }

        public string Error { get; set; }

        public static CommandStatus Complete(Guid id, string error = "")
        {
            return new CommandStatus
            {
                CommandId = id,
                Status = "Completed",
                Error = error
            };
        }

        public static CommandStatus Start(Guid id)
        {
            return new CommandStatus
            {
                CommandId = id,
                Status = "Started",
                Error = ""
            };
        }

        public static CommandStatus Queue(Guid id)
        {
            return new CommandStatus
            {
                CommandId = id,
                Status = "Queued",
                Error = ""
            };
        }
    }
}
