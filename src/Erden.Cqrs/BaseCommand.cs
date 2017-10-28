using System;

using Newtonsoft.Json;

namespace Erden.Cqrs
{
    /// <summary>
    /// Base command realization
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        public BaseCommand()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Command ID
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; private set; }
        /// <summary>
        /// Call timestamp
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; private set; }
    }
}