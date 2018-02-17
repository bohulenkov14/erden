using System;
using System.Threading;
using System.Threading.Tasks;

using Erden.EventSourcing;
using Erden.Demo.Application.Events;
namespace Erden.Demo.Application.EventHandlers
{
    public class SlowestEventHandler : IEventHandler<OrderOpenedEvent>
    {
        public async Task Handle(OrderOpenedEvent @event)
        {
            Task.Run(() =>
            {
                Console.WriteLine("Started processing event - SlowestEventHandler");
                Thread.Sleep(9000);
                Console.WriteLine("Ended processing event - SlowestEventHandler");
            });
        }
    }
}
