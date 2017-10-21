using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Erden.Cqrs;
using Erden.Demo.Application.Requests;

namespace Erden.Demo.Application.Commands
{
    public class ReopenOrderCommand : DomainCommand
    {
        public ReopenOrderCommand(Guid entityId, ReopenOrderRequest request) : base(entityId)
        {
            NewCount = request.NewCount;
        }

        [JsonProperty("new_count")]
        public int NewCount { get; set; }
    }
}