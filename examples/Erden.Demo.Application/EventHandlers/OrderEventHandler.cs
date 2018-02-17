using System;
using System.Threading;
using System.Threading.Tasks;

using Erden.EventSourcing;
using Erden.Demo.Application.Events;

namespace Erden.Demo.Application.EventHandlers
{
    public class OrderEventHandler : IEventHandler<OrderOpenedEvent>
    {
        public async Task Handle(OrderOpenedEvent @event)
        {
            Console.WriteLine("Started processing event");
            await Task.Delay(3000);
            Console.WriteLine("Ended processing event");
        }
    }
}
