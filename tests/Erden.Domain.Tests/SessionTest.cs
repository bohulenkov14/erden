using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

using Erden.EventSourcing;

namespace Erden.Domain.Tests
{
    public sealed class SessionTest
    {
        [Fact]
        public async Task Aggregate_Should_Be_Saved_In_Storage_Test()
        {
            var provider = Configure();

            var id = Guid.NewGuid();
            var aggregate = new TestAggregate(id, "Lorem");

            var session = provider.GetService<ISession>();
            await session.Add(aggregate);
            await session.Commit(id);

            var storage = provider.GetService<IAggregateStorage>();
            var result = await storage.Get<TestAggregate>(id, 0);

            Assert.Equal("Lorem", result.TestProperty);
        }

        [Fact]
        public async Task Aggregate_Should_Be_Retrieved_From_Storage_Test()
        {
            var provider = Configure();
            var id = Guid.NewGuid();

            var eventstore = provider.GetService<IEventStore>();
            await eventstore.Add(new TestAggregateCreatedEvent(id, "Ipsum", 0), 0);
            await eventstore.Add(new TestPropertyChangedEvent(id, "Lorem", 1), 1);

            var session = provider.GetService<ISession>();
            var aggregate = await session.Get<TestAggregate>(id, 1);

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