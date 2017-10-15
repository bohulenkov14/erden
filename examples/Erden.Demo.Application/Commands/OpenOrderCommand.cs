using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Erden.Cqrs;
using Erden.Demo.Application.Requests;

namespace Erden.Demo.Application.Commands
{
    /// <summary>
    /// Command for creating order
    /// </summary>
    public class OpenOrderCommand : DomainCommand
    {
        public OpenOrderCommand(Guid entityId, CreateOrderRequest request) : base(entityId)
        {
            Position = request.Position;
            Count = request.Count;
        }

        [JsonProperty("position")]
        public string Position { get; private set; }
        [JsonProperty("count")]
        public int Count { get; private set; }

        public override Task Log()
        {
            return Task.CompletedTask;
        }
    }
}