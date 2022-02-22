using Autofac;
using Moneta.Events;
using System.Diagnostics;

namespace AccountService.Events
{
    public interface IEventDispatcher
    {
        void Dispatch<TEvent>(TEvent evnt) where TEvent : IEvent;
    }

    public class EventDispatcher : IEventDispatcher
    {
        private static readonly ActivitySource Activity = new(nameof(EventDispatcher));

        private readonly IComponentContext _Context;

        public EventDispatcher(IComponentContext context)
        {
            this._Context = context;
        }

        public void Dispatch<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            var handler = this._Context.Resolve<IEventHandler<TEvent>>();
            //https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
            using (Activity activity = Activity.StartActivity("Dispatching event", ActivityKind.Consumer))
            {
                handler.Handle(evnt);
            }
        }
    }
}
