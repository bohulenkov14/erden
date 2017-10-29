using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Erden.Domain.Exceptions;

namespace Erden.Domain
{
    /// <summary>
    /// Current session, where stores used aggregate roots
    /// </summary>
    public class Session : ISession
    {
        /// <summary>
        /// Persistent aggregate storage
        /// </summary>
        private readonly IAggregateStorage storage;
        /// <summary>
        /// Traking aggregates
        /// </summary>
        private static Dictionary<Guid, AggregateRoot> trackingAggregates
            = new Dictionary<Guid, AggregateRoot>();

        /// <summary>
        /// Initialize a new instance of <see cref="Session"/> class
        /// </summary>
        /// <param name="storage">Persistent aggregate storage</param>
        public Session(IAggregateStorage storage)
        {
            this.storage = storage;
        }

        /// <summary>
        /// Add aggregate to traking list
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="aggregate">Aggregate instance</param>
        public Task Add<T>(T aggregate) where T : AggregateRoot
        {
            trackingAggregates.Add(aggregate.Id, aggregate);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Commit changes for aggregate with specified ID
        /// </summary>
        /// <param name="id">Aggregate ID</param>
        public async Task Commit(Guid id)
        {
            try
            {
                await storage.Save(trackingAggregates[id], trackingAggregates[id].Version);
            }
            catch (Exception)
            {
                trackingAggregates.Remove(id);
                throw;
            }
        }

        /// <summary>
        /// Get aggregate
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="id">Aggregate ID</param>
        /// <param name="version">Aggregate version</param>
        /// <returns>Aggregate instance</returns>
        public async Task<T> Get<T>(Guid id, long version) where T : AggregateRoot
        {
            if (trackingAggregates.TryGetValue(id, out var aggregate))
            {
                if (version != -2 && aggregate.Version != version)
                    throw new ConcurrencyException(id);
                return aggregate as T;
            }

            aggregate = await storage.Get<T>(id, version);
            trackingAggregates.Add(id, aggregate);
            return aggregate as T;
        }
        /// <summary>
        /// Get aggregate
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="id">Aggregate ID</param>
        /// <returns>Aggregate instance</returns>
        public Task<T> Get<T>(Guid id) where T : AggregateRoot
        {
            return Get<T>(id, -2);
        }
    }
}