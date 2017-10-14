using System;
using System.Threading.Tasks;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Event interface
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Event ID
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Entity ID
        /// </summary>
        Guid EntityId { get; }
        /// <summary>
        /// Entity version
        /// </summary>
        Version Version { get; }
        /// <summary>
        /// Timestamp
        /// </summary>
        long Timestamp { get; }

        /// <summary>
        /// Log event
        /// </summary>
        Task Log();
    }
}