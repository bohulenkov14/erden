using System.Threading.Tasks;

namespace Erden.Cqrs
{
    /// <summary>
    /// Query handler interface
    /// </summary>
    /// <typeparam name="T">Query type</typeparam>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQueryHandler<T, TResult>
        where T : IQuery<TResult>
        where TResult : class
    {
        /// <summary>
        /// Handle query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Query result</returns>
        Task<TResult> Execute(T query);
    }
}