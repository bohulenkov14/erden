using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Erden.EventSourcing
{
    public sealed class InMemoryEventBus : IEventPublisher, IEventHandlerRegistrator
    {
        private readonly Dictionary<Type, List<Delegate>> handlers
            = new Dictionary<Type, List<Delegate>>();

        public void Publish<T>(T @event) where T : IEvent
        {
            Task.Run(() =>
            {
                if (handlers.TryGetValue(@event.GetType(), out var list))
                {
                    foreach (var handler in list)
                    {
                        var result = handler.DynamicInvoke(@event) as Task;
                        if (result.Status == TaskStatus.Faulted)
                            throw result.Exception.InnerException;
                    }
                }
            });
        }

        public void Register<T>(Func<T, Task> handler) where T : IEvent
        {
            if (!handlers.TryGetValue(typeof(T), out var list))
            {
                list = new List<Delegate>();
                handlers.Add(typeof(T), list);
            }
            list.Add(handler);
        }
    }
}
