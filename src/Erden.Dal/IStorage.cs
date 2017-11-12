using System.Threading.Tasks;

namespace Erden.Dal
{
    /// <summary>
    /// Storage interface
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Retrieve data
        /// </summary>
        /// <typeparam name="TResult">Data type</typeparam>
        /// <param name="request">Fetch request</param>
        /// <returns>Result</returns>
        Task<TResult> Retrieve<TResult>(IFetchRequest<TResult> request) where TResult : class;
        /// <summary>
        /// Retrieve data
        /// </summary>
        /// <typeparam name="T">Fetch request type</typeparam>
        /// <typeparam name="TResult">Data type</typeparam>
        /// <returns>Result</returns>
        Task<TResult> Retrieve<T, TResult>()
            where TResult : class
            where T : IFetchRequest<TResult>;
        /// <summary>
        /// Retrieve data
        /// </summary>
        /// <typeparam name="T">Fetch request type</typeparam>
        /// <typeparam name="TResult">Data type</typeparam>
        /// <param name="args">Args to create fetch request</param>
        /// <returns>Result</returns>
        Task<TResult> Retrieve<T, TResult>(params object[] args)
            where TResult : class
            where T : IFetchRequest<TResult>;
    }
}