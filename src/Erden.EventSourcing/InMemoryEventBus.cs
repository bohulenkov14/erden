using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Default realization of <see cref="IEventPublisher"/> and <see cref="IEventHandlerRegistrator"/>
    /// </summary>
    public sealed class InMemoryEventBus : IEventPublisher, IEventHandlerRegistrator
    {
        /// <summary>
        /// Event handlers
        /// </summary>
        private readonly Dictionary<Type, List<Delegate>> handlers
            = new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Publish event
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="event">Event to publish</param>
        public void Publish<T>(T @event) where T : IEvent
        {
            if (handlers.TryGetValue(@event.GetType(), out var list))
            {
                var handleTasks = list
                    .Select(handler => (Task)handler.DynamicInvoke(@event))
                    .ToArray();

                Task.WaitAll(handleTasks);
            }
        }
        /// <summary>
        /// Register handler for events of provided type
        /// </summary>
        /// <typeparam name="T">Events type</typeparam>
        /// <param name="handler">Handler</param>
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
