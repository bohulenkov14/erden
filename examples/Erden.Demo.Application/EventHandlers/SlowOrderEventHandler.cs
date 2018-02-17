using System;
using System.Threading;
using System.Threading.Tasks;

using Erden.EventSourcing;
using Erden.Demo.Application.Events;

namespace Erden.Demo.Application.EventHandlers
{
    public class SlowOrderEventHandler : IEventHandler<OrderOpenedEvent>
    {
        public async Task Handle(OrderOpenedEvent @event)
        {
            Console.WriteLine("Started processing event - SlowOrderEventHandler");
            await Task.Delay(4500);
            Console.WriteLine("Ended processing event - SlowOrderEventHandler");
        }
    }
}
