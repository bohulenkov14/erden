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
    }
}