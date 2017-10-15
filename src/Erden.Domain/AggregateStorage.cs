using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Erden.Domain.Exceptions;
using Erden.EventSourcing;

namespace Erden.Domain
{
    public sealed class AggregateStorage : IAggregateStorage
    {
        private readonly IEventStore eventStore;

        public AggregateStorage(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task<T> Get<T>(Guid id, long version) where T : AggregateRoot
        {
            IEnumerable<IEvent> events = await LoadEvents(id, version);
            return LoadFromEvents<T>(events);
        }

        public async Task Save<T>(T aggregate, long version) where T : AggregateRoot
        {
            await eventStore.Add(aggregate.FlushCommit(), version);
        }

        private async Task<IEnumerable<IEvent>> LoadEvents(Guid id, long expectedVersion)
        {
            var events = await eventStore.Get(id);
            if (expectedVersion != -2 && events.Last().Version != expectedVersion)
                throw new ConcurrencyException(id);

            return events;
        }

        private T LoadFromEvents<T>(IEnumerable<IEvent> events) where T : AggregateRoot
        {
            var aggregate = Activator.CreateInstance(typeof(T)) as T;
            aggregate.LoadFromHistory(events);
            return aggregate;
        }
    }
}