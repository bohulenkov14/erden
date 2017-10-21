using Erden.Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erden.Demo.Application.Commands
{
    public class DeclineOrderCommand : DomainCommand
    {
        public DeclineOrderCommand(Guid entityId) : base(entityId) { }
    }
}