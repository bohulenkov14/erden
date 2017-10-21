using System;
using System.Threading.Tasks;

namespace Erden.Dal
{
    public interface IChangeHandlerRegistrator
    {
        void Register<T>(Func<T, Task> handler) where T : IChangeRequest;
    }
}