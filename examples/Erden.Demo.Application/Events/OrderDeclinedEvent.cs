using System;
using System.Threading.Tasks;

using Erden.EventSourcing;

namespace Erden.Demo.Application.Events
{
    public class OrderDeclinedEvent : BaseEvent
    {
        public OrderDeclinedEvent(Guid entityId, long version = -2)
            : base(entityId, version)
        { }
    }
}