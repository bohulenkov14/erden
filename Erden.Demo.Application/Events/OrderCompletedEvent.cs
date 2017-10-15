using System;
using System.Threading.Tasks;

using Erden.EventSourcing;

namespace Erden.Demo.Application.Events
{
    public class OrderCompletedEvent : BaseEvent
    {
        public OrderCompletedEvent(Guid entityId, long version = -2) 
            : base(entityId, version)
        { }

        public override Task Log()
        {
            return Task.CompletedTask;
        }
    }
}