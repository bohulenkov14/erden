﻿using System;
using System.Threading.Tasks;

using Erden.EventSourcing;

namespace Erden.Domain.Tests
{
    internal sealed class TestAggregate : AggregateRoot
    {
        public TestAggregate() { }

        public TestAggregate(Guid id, string testProperty)
        {
            ApplyChange<TestAggregateCreatedEvent>(id, testProperty);
        }

        public string TestProperty { get; private set; }

        public void ChangeTestProperty(string newTestPropertyValue)
        {
            ApplyChange<TestPropertyChangedEvent>(Id, newTestPropertyValue);
        }

        private void Apply(TestAggregateCreatedEvent @event)
        {
            Id = @event.EntityId;
            TestProperty = @event.TestProperty;
        }

        private void Apply(TestPropertyChangedEvent @event)
        {
            TestProperty = @event.NewValue;
        }
    }

    internal sealed class TestAggregateCreatedEvent : BaseEvent
    {
        public TestAggregateCreatedEvent(Guid entityId, string testProperty, long version)
            : base(entityId, version)
        {
            TestProperty = testProperty;
        }

        public string TestProperty { get; private set; }

        public override Task Log()
        {
            return Task.CompletedTask;
        }
    }

    internal sealed class TestPropertyChangedEvent : BaseEvent
    {
        public TestPropertyChangedEvent(Guid entityId, string newValue, long version = -2)
            : base(entityId, version)
        {
            NewValue = newValue;
        }

        public string NewValue { get; private set; }

        public override Task Log()
        {
            return Task.CompletedTask;
        }
    }
}