using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Erden.EventSourcing;

namespace Erden.Demo.Application.Events
{
    public class OrderResponsibleSetEvent : BaseEvent
    {
        public OrderResponsibleSetEvent(Guid entityId, Guid responsibleId, long version = -2) : base(entityId, version)
        {
            ResponsibleId = responsibleId;
        }

        [JsonProperty("responsible_id")]
        public Guid ResponsibleId { get; set; }

        public override Task Log()
        {
            return Task.CompletedTask;
        }
    }
}