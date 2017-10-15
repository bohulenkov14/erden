using Newtonsoft.Json;

namespace Erden.EventSourcing.Targets.EventStore
{
    /// <summary>
    /// Metadata for EventStore events
    /// </summary>
    internal sealed class EventMetadata
    {
        /// <summary>
        /// Full type name, for deserialization
        /// </summary>
        [JsonProperty("event_type")]
        public string AssemblyQualifiedName { get; set; }
    }
}