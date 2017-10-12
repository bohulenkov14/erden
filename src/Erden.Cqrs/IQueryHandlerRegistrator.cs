using System;
using System.Threading.Tasks;

namespace Erden.Cqrs
{
    /// <summary>
    /// Query handler registrar interface
    /// </summary>
    public interface IQueryHandlerRegistrator
    {
        /// <summary>
        /// Register query handler
        /// </summary>
        /// <typeparam name="T">Query type</typeparam>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="handler">Query handler</param>
        void Register<T, TResult>(Func<T, Task<TResult>> handler)
            where T : IQuery<TResult>
            where TResult : class;
    }
}
