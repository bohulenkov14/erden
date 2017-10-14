using System.Threading.Tasks;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Event handler interface
    /// </summary>
    /// <typeparam name="T">Event type</typeparam>
    public interface IEventHandler<T> where T : IEvent
    {
        /// <summary>
        /// Handling event
        /// </summary>
        /// <param name="event">Event</param>
        Task Handle(T @event);
    }
}