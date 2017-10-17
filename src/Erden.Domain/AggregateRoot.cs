using System;
using System.Collections.Generic;

using Erden.Domain.Exceptions;
using Erden.Domain.Infrastructure;
using Erden.EventSourcing;
using System.Linq;

namespace Erden.Domain
{
    public abstract class AggregateRoot
    {
        private readonly Commit commit = new Commit();

        public Guid Id { get; protected set; }
        public long Version { get; protected set; } = -1;

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                if (e.Version != Version + 1)
                    throw new EventsOutOfOrderException(e.Id);

                ApplyChange(e, false);
            }
        }

        public IEvent[] FlushCommit()
        {
            var changes = commit.Flush();
            Version += changes.Length;
            return changes;
        }

        protected void ApplyChange<T>(params object[] args)
        {
            var @event = Activator.CreateInstance(typeof(T), args.Concat(new object[] { Version + 1 }).ToArray()) as IEvent;
            ApplyChange(@event, true);
        }

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