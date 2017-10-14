using System;
using System.Threading.Tasks;

namespace Erden.Domain
{
    public interface IAggregateStorage
    {
        Task<T> Get<T>(Guid id, long version) where T : AggregateRoot;
        Task Save<T>(T aggregate, long version) where T : AggregateRoot;
    }
}