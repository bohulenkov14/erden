using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Erden.EventSourcing;

namespace Erden.Demo.Application.Events
{
    public class OrderOpenedEvent : BaseEvent
    {
        public OrderOpenedEvent(Guid entityId, string position, int count, long version) : base(entityId, version)
        {
            Position = position;
            Count = count;
        }

        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}