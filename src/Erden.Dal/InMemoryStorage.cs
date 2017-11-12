using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Erden.Dal.Exceptions;

namespace Erden.Dal
{
    /// <summary>
    /// Default realization of <see cref="IStorage"/> and <see cref="IFetchHandlerRegistrator"/>
    /// </summary>
    public sealed class InMemoryStorage : IStorage, IFetchHandlerRegistrator
    {
        /// <summary>
        /// Handlers
        /// </summary>
        private readonly Dictionary<Type, Delegate> handlers
            = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Register handler
        /// </summary>
        /// <typeparam name="T">Fetch request type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="handler">Handler</param>
        public void Register<T, TResult>(Func<T, Task<TResult>> handler)
            where T : IFetchRequest<TResult>
            where TResult : class
        {
            if (handler == null)
                throw new ArgumentNullException("handler");
            handlers.Add(typeof(T), handler);
        }

        /// <summary>
        /// Retrieve data
        /// </summary>
        /// <typeparam name="TResult">Data type</typeparam>
        /// <param name="request">Fetch request</param>
        /// <returns>Result</returns>
        public Task<TResult> Retrieve<TResult>(IFetchRequest<TResult> request)
            where TResult : class
        {
            if (handlers.TryGetValue(request.GetType(), out var handler))
            {
                return handler.DynamicInvoke(request) as Task<TResult>;
            }

            throw new FetchHandlerNotFoundException(request.GetType());
        }
        /// <summary>
        /// Retrieve data
        /// </summary>
        /// <typeparam name="T">Fetch request type</typeparam>
        /// <typeparam name="TResult">Data type</typeparam>
        /// <returns>Result</returns>
        public Task<TResult> Retrieve<T, TResult>()
            where T : IFetchRequest<TResult>
            where TResult : class
        {
            return Retrieve((T)Activator.CreateInstance(typeof(T)));
        }
        /// <summary>
        /// Retrieve data
        /// </summary>
        /// <typeparam name="T">Fetch request type</typeparam>
        /// <typeparam name="TResult">Data type</typeparam>
        /// <param name="args">Args to create fetch request</param>
        /// <returns>Result</returns>
        public Task<TResult> Retrieve<T, TResult>(params object[] args)
            where T : IFetchRequest<TResult>
            where TResult : class
        {
            return Retrieve((T)Activator.CreateInstance(typeof(T), args));
        }
    }
}