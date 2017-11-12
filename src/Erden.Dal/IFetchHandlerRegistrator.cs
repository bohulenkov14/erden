using System;
using System.Threading.Tasks;

namespace Erden.Dal
{
    /// <summary>
    /// Fetch request handler interface
    /// </summary>
    public interface IFetchHandlerRegistrator
    {
        /// <summary>
        /// Register handler
        /// </summary>
        /// <typeparam name="T">Fetch request type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="handler">Handler</param>
        void Register<T, TResult>(Func<T, Task<TResult>> handler)
            where T : IFetchRequest<TResult>
            where TResult : class;
    }
}