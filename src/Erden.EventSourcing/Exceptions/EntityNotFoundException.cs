using System;

namespace Erden.EventSourcing.Exceptions
{
    /// <summary>
    /// Represents error that occur when trying get non existing entity from eventstore
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Guid id)
            : base($"Entity with ID {id} not found in EventStore")
        { }
    }
}