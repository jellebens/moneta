using Moneta.Events;
using System.Threading.Tasks;

namespace AccountService.Events
{
    public interface IEventHandler<TEvent>  where TEvent: IEvent
    {
        public Task Handle(TEvent evnt);
    }
}
