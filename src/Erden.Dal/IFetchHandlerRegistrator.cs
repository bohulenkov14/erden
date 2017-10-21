using System;
using System.Threading.Tasks;

namespace Erden.Dal
{
    public interface IFetchHandlerRegistrator
    {
        void Register<T, TResult>(Func<T, Task<TResult>> handler)
            where T : IFetchRequest<TResult>
            where TResult : class;
    }
}