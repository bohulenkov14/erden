using System;
using System.Threading.Tasks;

namespace Erden.Domain
{
    public interface ISession
    {
        Task Add<T>(T aggregate) where T : AggregateRoot;
        Task<T> Get<T>(Guid id, long version) where T : AggregateRoot;
        Task Commit(Guid id);
    }
}