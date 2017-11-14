using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Xunit;

using Erden.Configuration;

namespace Erden.EventSourcing.Tests
{
    public class InMemoryEventBusTest
    {
        [Fact]
        public void Event_Should_Be_Handled_Test()
        {
            var provider = Configure();

            IEventPublisher publisher = provider.GetService<IEventPublisher>();
            publisher.Publish(new FirstTestEvent(Guid.NewGuid()));
        }

        [Fact]
        public void Different_Events_Should_Be_Handled()
        {
            var provider = Configure();

            IEventPublisher publisher = provider.GetService<IEventPublisher>();
            publisher.Publish(new FirstTestEvent(Guid.NewGuid()));
            publisher.Publish(new SecondTestEvent(Guid.NewGuid()));
        }

        private ServiceProvider Configure()
        {
            var services = new ServiceCollection();
            var config = new ErdenConfig(services)
                .AddEventSourcing()
                .UseDefaultEventBus();
            config.Build();

            return services.BuildServiceProvider();
        }
    }

    internal sealed class FirstTestEvent : BaseEvent
    {
        public FirstTestEvent(Guid entityId, long version = -2)
            : base(entityId, version)
        { }
    }

    internal sealed class SecondTestEvent : BaseEvent
    {
        public SecondTestEvent(Guid entityId, long version = -2)
            : base(entityId, version)
        { }
    }

    internal sealed class FirstTestEventHandler : IEventHandler<FirstTestEvent>
    {
        public Task Handle(FirstTestEvent @event)
        {
            return Task.CompletedTask;
        }
    }

    internal sealed class SecondTestEventHandler : IEventHandler<SecondTestEvent>
    {
        public Task Handle(SecondTestEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}