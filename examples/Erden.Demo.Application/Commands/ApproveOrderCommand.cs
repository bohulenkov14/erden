using System;
using System.Threading.Tasks;

using Erden.Cqrs;

namespace Erden.Demo.Application.Commands
{
    public class ApproveOrderCommand : DomainCommand
    {
        public ApproveOrderCommand(Guid entityId) : base(entityId) { }
    }
}