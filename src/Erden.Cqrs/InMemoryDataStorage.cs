using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Erden.Cqrs.Exceptions;

namespace Erden.Cqrs
{
    /// <summary>
    /// Default realization of <see cref="IDataStorage"/> and <see cref="IQueryHandlerRegistrator"/>
    /// Provides query handler registration and query execution
    /// </summary>
    public sealed class InMemoryDataStorage : IDataStorage, IQueryHandlerRegistrator
    {
        /// <summary>
        /// Query handlers
        /// </summary>
        private readonly Dictionary<Type, Delegate> handlers
            = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Register query handler
        /// </summary>
        /// <typeparam name="T">Query type</typeparam>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="handler">Query handler</param>
        public void Register<T, TResult>(Func<T, Task<TResult>> handler)
            where T : IQuery<TResult>
            where TResult : class
        {
            if (handler == null)
                throw new ArgumentNullException("handler");
            handlers.Add(typeof(T), handler);
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="query">Query</param>
        /// <returns>Query result</returns>
        public Task<T> Retrieve<T>(IQuery<T> query) where T : class
        {
            if (handlers.TryGetValue(query.GetType(), out var handler))
            {
                return handler.DynamicInvoke(query) as Task<T>;
            }

            throw new QueryHandlerNotFoundException(query.GetType());
        }
        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <returns>Query result</returns>
        public Task<TResult> Retrieve<T, TResult>(params object[] args)
            where T : IQuery<TResult>
            where TResult : class
        {
            return Retrieve((T)Activator.CreateInstance(typeof(T), args));
        }
        /// <summary>
        /// Execute query
        /// </summary>
        /// <typeparam name="TResult">Query result type</typeparam>
        /// <param name="args">Query's constructor args</param>
        /// <returns>Query result</returns>
        public Task<TResult> Retrieve<T, TResult>()
            where T : IQuery<TResult>
            where TResult : class
        {
            return Retrieve((T)Activator.CreateInstance(typeof(T)));
        }
    }
}