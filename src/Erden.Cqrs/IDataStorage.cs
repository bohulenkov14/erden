using System.Threading.Tasks;

namespace Erden.Cqrs
{
    /// <summary>
    /// Data storage interface
    /// </summary>
    public interface IDataStorage
    {
        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="query">Query</param>
        /// <returns>Query result</returns>
        Task<TResult> Retrieve<TResult>(IQuery<TResult> query) where TResult : class;
        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <returns>Query result</returns>
        Task<TResult> Retrieve<T, TResult>()
            where TResult : class
            where T : IQuery<TResult>;
        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="args">Query's constructor args</param>
        /// <returns>Query result</returns>
        Task<TResult> Retrieve<T, TResult>(params object[] args)
            where TResult : class
            where T : IQuery<TResult>;
    }
}