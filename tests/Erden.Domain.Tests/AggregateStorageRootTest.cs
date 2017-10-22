using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

using Erden.EventSourcing;

namespace Erden.Domain.Tests
{
    public sealed class AggregateStorageRootTest
    {
        [Fact]
        public async Task Aggregate_Events_Should_Be_Saved_Test()
        {
            var provider = Configure();
            var id = Guid.NewGuid();
            var aggregate = new TestAggregate(id, "Lorem");

            var storage = provider.GetService<IAggregateStorage>();
            await storage.Save(aggregate, -2);

            var eventstore = provider.GetService<IEventStore>();

            var events = await eventstore.Get(id);
            Assert.Equal(1, events.Count);

            var @event = events.First() as TestAggregateCreatedEvent;
            Assert.Equal("Lorem", @event.TestProperty);
        }

        [Fact]
        public async Task Aggregate_Should_Be_Restored_From_History()
        {
            var provider = Configure();
            var id = Guid.NewGuid();

            var eventstore = provider.GetService<IEventStore>();
            await eventstore.Add(new TestAggregateCreatedEvent(id, "Ipsum", 0), 0);
            await eventstore.Add(new TestPropertyChangedEvent(id, "Lorem", 1), 1);

            var storage = provider.GetService<IAggregateStorage>();
            var aggregate = await storage.Get<TestAggregate>(id, 1);

            Assert.Equal("Lorem", aggregate.TestProperty);
        }

        private ServiceProvider Configure()
        {
            var services = new ServiceCollection();
            var config = new ESConfiguration(services)
                .UseDefaultEventBus()
                .UseTestEventStore();
            config.Build();
            new DomainConfiguration(services).Build();

            return services.BuildServiceProvider();
        }
    }
}