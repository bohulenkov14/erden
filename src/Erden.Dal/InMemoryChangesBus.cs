using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Erden.Dal.Exceptions;

namespace Erden.Dal
{
    /// <summary>
    /// Default realization of <see cref="IChangesBus"/> and <see cref="IChangeHandlerRegistrator"/>
    /// </summary>
    public sealed class InMemoryChangesBus : IChangesBus, IChangeHandlerRegistrator
    {
        /// <summary>
        /// Handlers
        /// </summary>
        private readonly Dictionary<Type, Func<IChangeRequest, Task>> handlers
            = new Dictionary<Type, Func<IChangeRequest, Task>>();

        /// <summary>
        /// Register handler
        /// </summary>
        /// <typeparam name="T">Change request type</typeparam>
        /// <param name="handler">Handler</param>
        public void Register<T>(Func<T, Task> handler) where T : IChangeRequest
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            handlers.Add(typeof(T), x => handler((T)x));
        }
        /// <summary>
        /// Send request for execution
        /// </summary>
        /// <typeparam name="T">Change request type</typeparam>
        /// <param name="request">Change request</param>
        public async Task Send<T>(T request) where T : IChangeRequest
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (!handlers.TryGetValue(typeof(T), out var handler))
                throw new ChangeHandlerNotFoundException(typeof(T));

            await handler.Invoke(request);
        }
        /// <summary>
        /// Send request for execution
        /// </summary>
        /// <typeparam name="T">Change request type</typeparam>
        public Task Send<T>() where T : IChangeRequest
        {
            return Send((T)Activator.CreateInstance(typeof(T)));
        }
        /// <summary>
        /// Send request for execution
        /// </summary>
        /// <typeparam name="T">Change request type</typeparam>
        /// <param name="args">Args to create change request</param>
        public Task Send<T>(params object[] args) where T : IChangeRequest
        {
            return Send((T)Activator.CreateInstance(typeof(T), args));
        }
    }
}