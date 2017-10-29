using System;
using System.Threading.Tasks;

namespace Erden.Domain
{
    /// <summary>
    /// Persistante aggregate storage
    /// </summary>
    public interface IAggregateStorage
    {
        /// <summary>
        /// Get aggregate
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="id">Aggregate ID</param>
        /// <param name="version">Aggregate version</param>
        /// <returns>Aggregate instance-</returns>
        Task<T> Get<T>(Guid id, long version) where T : AggregateRoot;
        /// <summary>
        /// Save aggregate changes
        /// </summary>
        /// <typeparam name="T">Aggregate type</typeparam>
        /// <param name="aggregate">Aggregate instance</param>
        /// <param name="version">Aggregate version</param>
        Task Save<T>(T aggregate, long version) where T : AggregateRoot;
    }
}