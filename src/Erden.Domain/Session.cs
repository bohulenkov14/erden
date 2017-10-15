using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Erden.Domain.Exceptions;

namespace Erden.Domain
{
    public class Session : ISession
    {
        private readonly IAggregateStorage storage;
        private static Dictionary<Guid, AggregateRoot> trackingAggregates
            = new Dictionary<Guid, AggregateRoot>();

        public Session(IAggregateStorage storage)
        {
            this.storage = storage;
        }

        public Task Add<T>(T aggregate) where T : AggregateRoot
        {
            trackingAggregates.Add(aggregate.Id, aggregate);
            return Task.CompletedTask;
        }

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
    }
}