using System;

using Newtonsoft.Json;

namespace Erden.Cqrs
{
    /// <summary>
    /// Base query realization
    /// </summary>
    /// <typeparam name="TResult">Query result</typeparam>
    public abstract class BaseQuery<TResult> : IQuery<TResult> where TResult : class
    {
        public BaseQuery()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Query ID
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