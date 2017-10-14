using System;

namespace Erden.Domain.Exceptions
{
    /// <summary>
    /// Represent error that occur when history events from EventStore have wrong order
    /// </summary>
    public class EventsOutOfOrderException : Exception
    {
        public EventsOutOfOrderException(Guid id)
            : base($"History events for aggregate {id} out of order")
        { }
    }
}