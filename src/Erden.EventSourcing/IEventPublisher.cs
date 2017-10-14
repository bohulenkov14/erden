namespace Erden.EventSourcing
{
    /// <summary>
    /// Events publisher interface
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publish event
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="event">Event to publish</param>
        void Publish<T>(T @event) where T : IEvent;
    }
}