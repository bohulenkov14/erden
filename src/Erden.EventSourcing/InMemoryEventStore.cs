using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Erden.EventSourcing.Exceptions;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Only for tests and demo
    /// In memory implementation of <see cref="IEventStore"/>
    /// </summary>
    public sealed class InMemoryEventStore : IEventStore
    {
        private readonly IEventPublisher publisher;
        private static Dictionary<Guid, List<IEvent>> store
            = new Dictionary<Guid, List<IEvent>>();

        /// <summary>
        /// Initialze a new instance of the <see cref="InMemoryEventStore"/> class
        /// </summary>
        /// <param name="publisher">Event publisher</param>
        public InMemoryEventStore(IEventPublisher publisher)
        {
            this.publisher = publisher;
        }

        public Task Add(IEvent @event, long version)
        {
            lock (store)
            {
                if (!store.TryGetValue(@event.EntityId, out var events))
                {
                    events = new List<IEvent>();
                    store.Add(@event.EntityId, events);
                }

                events.Add(@event);
                publisher.Publish(@event);
            }

            return Task.CompletedTask;
        }

        public Task Add(IReadOnlyCollection<IEvent> events, long version)
        {
            lock (store)
            {
                var id = events.First().EntityId;

                if (!store.TryGetValue(id, out var eventsList))
                {
                    eventsList = new List<IEvent>();
                    store.Add(id, eventsList);
                }

                eventsList.AddRange(events);
                Task.Run(() =>
                {
                    foreach (var @event in events)
                        publisher.Publish(@event);
                });
            }

            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<IEvent>> Get(Guid id)
        {
            return Task.Run(() =>
            {
                if (store.TryGetValue(id, out var events))
                {
                    return events.AsReadOnly() as IReadOnlyCollection<IEvent>;
                }

                throw new EntityNotFoundException(id);
            });
        }
    }
}