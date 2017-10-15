using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Erden.Cqrs;
using Erden.Demo.Application.Requests;

namespace Erden.Demo.Application.Commands
{
    public class SetOrderResponsibleCommand : DomainCommand
    {
        public SetOrderResponsibleCommand(Guid entityId, SetOrderResponsibleRequest request) : base(entityId)
        {
            ResponsibleId = request.ResponsibleId;
        }

        [JsonProperty("responsible_id")]
        public Guid ResponsibleId { get; private set; }

        public override Task Log()
        {
            return Task.CompletedTask;
        }
    }
}