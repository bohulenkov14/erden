using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erden.Dal
{
    public sealed class InMemoryChangesBus : IChangesBus, IChangeHandlerRegistrator
    {
        private readonly Dictionary<Type, Func<IChangeRequest, Task>> handlers
            = new Dictionary<Type, Func<IChangeRequest, Task>>();

        public void Register<T>(Func<T, Task> handler) where T : IChangeRequest
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            handlers.Add(typeof(T), x => handler((T)x));
        }

        public async Task Send<T>(T request) where T : IChangeRequest
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (!handlers.TryGetValue(typeof(T), out var handler))
                throw new Exception();

            await handler.Invoke(request);
        }

        public Task Send<T>(params object[] args) where T : IChangeRequest
        {
            return Send((T)Activator.CreateInstance(typeof(T), args));
        }
    }
}