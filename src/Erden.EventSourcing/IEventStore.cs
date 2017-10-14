using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Event store interface
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Add event to eventstore
        /// </summary>
        /// <param name="event">Event</param>
        /// <param name="version">Entity version</param>
        Task Add(IEvent @event, long version);
        /// <summary>
        /// Add events to eventstore
        /// </summary>
        /// <param name="events">Collection of events</param>
        /// <param name="version">Entity version</param>
        Task Add(IReadOnlyCollection<IEvent> events, long version);
        /// <summary>
        /// Get entity's events
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>History events</returns>
        Task<IReadOnlyCollection<IEvent>> Get(Guid id);
    }
}
