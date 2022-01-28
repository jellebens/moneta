using Microsoft.AspNetCore.SignalR;
using Moneta.Frontend.Commands;
using System;
using System.Threading.Tasks;

namespace Moneta.Frontend.API.Hubs
{
    public class CommandHub : Hub
    {
        public async Task Update(Guid id, CommandStatus status)
        {
            await Clients.All.SendAsync(id.ToString(), status);
        }
    }
}
