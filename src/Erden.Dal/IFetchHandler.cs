using System.Threading.Tasks;

namespace Erden.Dal
{
    /// <summary>
    /// Fetch handler interface
    /// </summary>
    /// <typeparam name="TFetch">Fetch request type</typeparam>
    /// <typeparam name="TResult">Result type</typeparam>
    public interface IFetchHandler<TFetch, TResult>
        where TFetch : IFetchRequest<TResult>
        where TResult : class
    {
        /// <summary>
        /// Execute fetch request
        /// </summary>
        /// <param name="request">Fetch request</param>
        /// <returns>Fetch result</returns>
        Task<TResult> Execute(TFetch request);
    }
}