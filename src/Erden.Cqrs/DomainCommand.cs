using System;

namespace Erden.Cqrs
{
    public abstract class DomainCommand : BaseCommand
    {
        public DomainCommand(Guid entityId)
        {
            EntityId = entityId;
        }

        public Guid EntityId { get; private set; }
    }
}