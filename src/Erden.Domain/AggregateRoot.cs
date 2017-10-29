using System;
using System.Collections.Generic;
using System.Linq;

using Erden.Domain.Exceptions;
using Erden.Domain.Infrastructure;
using Erden.EventSourcing;

namespace Erden.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AggregateRoot
    {
        /// <summary>
        /// Unsaved changes
        /// </summary>
        private readonly Commit commit = new Commit();

        /// <summary>
        /// Aggregate ID
        /// </summary>
        public Guid Id { get; protected set; }
        /// <summary>
        /// Current aggregate version
        /// </summary>
        public long Version { get; protected set; } = -1;

        /// <summary>
        /// Restore aggregate state from events
        /// </summary>
        /// <param name="history"></param>
        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                if (e.Version != Version + 1)
                    throw new EventsOutOfOrderException(e.Id);

                ApplyChange(e, false);
            }
        }

        /// <summary>
        /// Take and clear changes
        /// </summary>
        /// <returns>Collection of unsaved events</returns>
        public IEvent[] FlushCommit()
        {
            var changes = commit.Flush();
            Version += changes.Length;
            return changes;
        }

        /// <summary>
        /// Apply change for aggregate
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="args">Event args</param>
        protected void ApplyChange<T>(params object[] args)
        {
            var @event = Activator.CreateInstance(typeof(T), args.Concat(new object[] { Version + 1 }).ToArray()) as IEvent;
            ApplyChange(@event, true);
        }

        /// <summary>
        /// Apply changes
        /// </summary>
        /// <param name="event">Event</param>
        /// <param name="isNew">Is event new</param>
        private void ApplyChange(IEvent @event, bool isNew)
        {
            lock (commit)
            {
                this.CallApply(@event);

                if (isNew)
                    commit.Add(@event);
                else
                    ++Version;
            }
        }
    }
}