using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Erden.Demo.Application.Requests;
using Erden.Cqrs;
using Erden.Demo.Application.Commands;

namespace Erden.Demo.Application.Controllers
{
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly ICommandBus bus;

        public OrdersController(ICommandBus bus)
        {
            this.bus = bus;
        }

        [HttpPost]
        public async Task<Guid> Post([FromBody] CreateOrderRequest request)
        {
            Console.WriteLine("Started processing request");
            var id = Guid.NewGuid();
            await bus.Send(new OpenOrderCommand(id, request));
            Console.WriteLine("Started processing request");
            return id;
        }

        [HttpPost("{id}/approve")]
        public async Task Approve(Guid id)
        {
            await bus.Send(new ApproveOrderCommand(id));
        }

        [HttpPost("{id}/decline")]
        public async Task Decline(Guid id)
        {
            await bus.Send(new DeclineOrderCommand(id));
        }

        [HttpPost("{id}/close")]
        public async Task Close(Guid id)
        {
            await bus.Send(new CloseOrderCommand(id));
        }

        [HttpPost("{id}/complete")]
        public async Task Complete(Guid id)
        {
            await bus.Send(new CompleteOrderCommand(id));
        }

        [HttpPost("{id}")]
        public async Task Reopen(Guid id, [FromBody] ReopenOrderRequest request)
        {
            await bus.Send(new ReopenOrderCommand(id, request));
        }

        [HttpPost("{id}")]
        public async Task SetResponsible(Guid id, [FromBody] SetOrderResponsibleRequest request)
        {
            await bus.Send(new SetOrderResponsibleCommand(id, request));
        }
    }
}
