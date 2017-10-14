using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Erden.EventSourcing
{
    /// <summary>
    /// Base event implementation
    /// </summary>
    public abstract class BaseEvent : IEvent
    {
        public BaseEvent(Guid entityId, long version = (long)EventSourcing.Version.Any)
        {
            Id = Guid.NewGuid();
            EntityId = entityId;
            Version = version;
        }

        [JsonProperty("id")]
        public Guid Id { get; private set; }
        [JsonProperty("entity_id")]
        public Guid EntityId { get; private set; }
        [JsonProperty("version")]
        public long Version { get; private set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; private set; }

        public abstract Task Log();
    }
}