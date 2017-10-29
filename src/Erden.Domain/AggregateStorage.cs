using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Erden.Domain.Exceptions;
using Erden.EventSourcing;

namespace Erden.Domain
{
    /// <summary>
    /// Persistante aggregate storage
    /// </summary>
    public sealed class AggregateStorage : IAggregateStorage
    {
        /// <summary>
        /// Event store
        /// </summary>
        private readonly IEventStore eventStore;

        /// <summary>
        /// Initialize a new instance of <see cref="AggregateStorage"/> class
        /// </summary>
        /// <param name="eventStore">Event store</param>
        public AggregateStorage(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        /// <summary>
        /// Get aggregate
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="id">Aggregate ID</param>
        /// <param name="version">Aggregate version</param>
        /// <returns>Aggregate instance-</returns>
        public async Task<T> Get<T>(Guid id, long version) where T : AggregateRoot
        {
            IEnumerable<IEvent> events = await LoadEvents(id, version);
            return LoadFromEvents<T>(events);
        }
        /// <summary>
        /// Save aggregate changes
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="aggregate">Aggregate instance</param>
        /// <param name="version">Aggregate version</param>
        public async Task Save<T>(T aggregate, long version) where T : AggregateRoot
        {
            await eventStore.Add(aggregate.FlushCommit(), version);
        }

        /// <summary>
        /// Load events for restoring aggregate state
        /// </summary>
        /// <param name="id">Aggregate ID</param>
        /// <param name="expectedVersion">Expected aggregate version</param>
        /// <returns>Collection of events</returns>
        private async Task<IEnumerable<IEvent>> LoadEvents(Guid id, long expectedVersion)
        {
            var events = await eventStore.Get(id);
            if (expectedVersion != -2 && events.Last().Version != expectedVersion)
                throw new ConcurrencyException(id);

            return events;
        }

        /// <summary>
        /// Restore aggregate from events
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="events">Aggregate events</param>
        /// <returns>Aggregate instance</returns>
        private T LoadFromEvents<T>(IEnumerable<IEvent> events) where T : AggregateRoot
        {
            var aggregate = Activator.CreateInstance(typeof(T)) as T;
            aggregate.LoadFromHistory(events);
            return aggregate;
        }
    }
}