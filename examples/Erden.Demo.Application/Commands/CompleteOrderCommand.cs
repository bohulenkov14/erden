using Erden.Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erden.Demo.Application.Commands
{
    public class CompleteOrderCommand : DomainCommand
    {
        public CompleteOrderCommand(Guid entityId) : base(entityId) { }
    }
}