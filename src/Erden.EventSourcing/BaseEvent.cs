using System;

using Newtonsoft.Json;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Base event implementation
    /// </summary>
    public abstract class BaseEvent : IEvent
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="BaseEvent"/> class with ID and current version of entity
        /// </summary>
        /// <param name="entityId">Entity ID</param>
        /// <param name="version">Current version</param>
        public BaseEvent(Guid entityId, long version = (long)EventSourcing.Version.Any)
        {
            Id = Guid.NewGuid();
            EntityId = entityId;
            Version = version;
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Event ID
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; private set; }
        /// <summary>
        /// Entity ID
        /// </summary>
        [JsonProperty("entity_id")]
        public Guid EntityId { get; private set; }
        /// <summary>
        /// Entity version
        /// </summary>
        [JsonProperty("version")]
        public long Version { get; private set; }
        /// <summary>
        /// Call timestamp
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; private set; }
    }
}