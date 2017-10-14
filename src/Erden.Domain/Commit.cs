using System;
using System.Collections.Generic;

using Erden.EventSourcing;

namespace Erden.Domain
{
    /// <summary>
    /// Aggregate root commit
    /// </summary>
    internal sealed class Commit
    {
        /// <summary>
        /// Not commited events
        /// </summary>
        private readonly List<IEvent> events = new List<IEvent>();

        /// <summary>
        /// Add event to commit
        /// </summary>
        /// <param name="commit">Applied event</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="CommitNotFlushedException"></exception>
        public void Add(IEvent commit)
        {
            if (commit == null)
                throw new ArgumentNullException("commit");

            lock (events)
                events.Add(commit);
        }

        /// <summary>
        /// Clear events before writing to EventStore
        /// </summary>
        /// <returns>Commit event</returns>
        public IEvent[] Flush()
        {
            lock (events)
            {
                var changes = events.ToArray();
                events.Clear();
                return changes;
            }
        }
    }
}