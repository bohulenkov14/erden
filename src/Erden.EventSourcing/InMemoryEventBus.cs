using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Default realization of <see cref="IEventPublisher"/> and <see cref="IEventHandlerRegistrator"/>
    /// </summary>
    public sealed class InMemoryEventBus : IEventPublisher, IEventHandlerRegistrator
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger logger;
        /// <summary>
        /// Event handlers
        /// </summary>
        private readonly Dictionary<Type, List<Delegate>> handlers
            = new Dictionary<Type, List<Delegate>>();

        /// <summary>
        /// Initialize a new instance of the <see cref="InMemoryEventBus"/> class
        /// </summary>
        /// <param name="logger">Logger</param>
        public InMemoryEventBus(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Publish event
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="event">Event to publish</param>
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
                        {
                            logger.LogError(
                                result.Exception.InnerException,
                                $"Error while processing event of type {@event.GetType()}, event: {@event}"
                            );
                        }
                    }
                }
            });
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
