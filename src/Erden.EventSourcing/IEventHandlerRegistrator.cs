using System;
using System.Threading.Tasks;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Event handlers registrator interface
    /// </summary>
    public interface IEventHandlerRegistrator
    {
        /// <summary>
        /// Register handler for events of provided type
        /// </summary>
        /// <typeparam name="T">Events type</typeparam>
        /// <param name="handler">Handler</param>
        void Register<T>(Func<T, Task> handler) where T : IEvent;
    }
}