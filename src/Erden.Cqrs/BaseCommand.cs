using System;
using System.Threading.Tasks;

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
        }

        [JsonProperty("id")]
        public Guid Id { get; private set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; private set; }

        public abstract Task Log();
    }
}