using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erden.Dal
{
    public sealed class InMemoryStorage : IStorage, IFetchHandlerRegistrator
    {
        private readonly Dictionary<Type, Delegate> handlers
            = new Dictionary<Type, Delegate>();

        public void Register<T, TResult>(Func<T, Task<TResult>> handler)
            where T : IFetchRequest<TResult>
            where TResult : class
        {
            if (handler == null)
                throw new ArgumentNullException("handler");
            handlers.Add(typeof(T), handler);
        }

        public Task<TResult> Retrieve<TResult>(IFetchRequest<TResult> request)
            where TResult : class
        {
            if (handlers.TryGetValue(request.GetType(), out var handler))
            {
                return handler.DynamicInvoke(request) as Task<TResult>;
            }

            throw new Exception();
        }

        public Task<TResult> Retrieve<T, TResult>(params object[] args)
            where T : IFetchRequest<TResult>
            where TResult : class
        {
            return Retrieve((T)Activator.CreateInstance(typeof(T), args));
        }
    }
}