using System;
using System.Threading.Tasks;

namespace Erden.Domain
{
    /// <summary>
    /// Current session, where stores used aggregate roots
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Add aggregate to traking list
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="aggregate">Aggregate instance</param>
        Task Add<T>(T aggregate) where T : AggregateRoot;
        /// <summary>
        /// Get aggregate
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="id">Aggregate ID</param>
        /// <param name="version">Aggregate version</param>
        /// <returns>Aggregate instance</returns>
        Task<T> Get<T>(Guid id, long version) where T : AggregateRoot;
        /// <summary>
        /// Get aggregate
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="id">Aggregate ID</param>
        /// <returns>Aggregate instance</returns>
        Task<T> Get<T>(Guid id) where T : AggregateRoot;
        /// <summary>
        /// Commit changes for aggregate with specified ID
        /// </summary>
        /// <param name="id">Aggregate ID</param>
        Task Commit(Guid id);
    }
}