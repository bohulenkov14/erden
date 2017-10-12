using System;
using System.Threading.Tasks;

namespace Erden.Cqrs
{
    /// <summary>
    /// Data query interface
    /// </summary>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQuery<TResult> where TResult : class
    {
        /// <summary>
        /// Query ID
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Query timestamp
        /// </summary>
        long Timestamp { get; }

        /// <summary>
        /// Log query
        /// </summary>
        Task Log();
    }
}