using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Erden.EventSourcing;

namespace Erden.Demo.Application.Events
{
    public class OrderReopenedEvent : BaseEvent
    {
        public OrderReopenedEvent(Guid entityId, int newCount, long version = -2) : base(entityId, version)
        {
            NewCount = newCount;
        }

        [JsonProperty("new_count")]
        public int NewCount { get; set; }
    }
}