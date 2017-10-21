using System;
using System.Threading.Tasks;

using Erden.EventSourcing;

namespace Erden.Demo.Application.Events
{
    public class OrderClosedEvent : BaseEvent
    {
        public OrderClosedEvent(Guid entityId, long version = -2)
            : base(entityId, version)
        { }
    }
}