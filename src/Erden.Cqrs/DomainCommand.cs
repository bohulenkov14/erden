using System;

namespace Erden.Cqrs
{
    /// <summary>
    /// Domain command
    /// </summary>
    public abstract class DomainCommand : BaseCommand
    {
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="entityId">Entity ID</param>
        public DomainCommand(Guid entityId)
        {
            EntityId = entityId;
        }

        /// <summary>
        /// Entity ID
        /// </summary>
        public Guid EntityId { get; private set; }
    }
}