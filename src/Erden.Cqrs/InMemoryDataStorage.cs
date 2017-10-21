using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erden.Cqrs
{
    public sealed class InMemoryDataStorage : IDataStorage, IQueryHandlerRegistrator
    {
        private readonly Dictionary<Type, Delegate> handlers
            = new Dictionary<Type, Delegate>();

        public Task<T> Retrieve<T>(IQuery<T> query) where T : class
        {
            if (handlers.TryGetValue(query.GetType(), out var handler))
            {
                return handler.DynamicInvoke(query) as Task<T>;
            }

           throw new Exception();
        }

        public void Register<T, TResult>(Func<T, Task<TResult>> handler)
            where T : IQuery<TResult>
            where TResult : class
        {
            if (handler == null)
                throw new ArgumentNullException("handler");
            handlers.Add(typeof(T), handler);
        }
    }
}