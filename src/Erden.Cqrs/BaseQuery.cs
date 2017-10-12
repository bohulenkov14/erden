using System;
using System.Threading.Tasks;

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
        }

        [JsonProperty("id")]
        public Guid Id { get; private set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; private set; }

        public abstract Task Log();
    }
}