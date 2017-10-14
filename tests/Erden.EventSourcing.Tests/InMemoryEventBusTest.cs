using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
            var config = new ESConfiguration(services)
                .UseDefaultEventBus()
                .WithAssembly(typeof(InMemoryEventBusTest).GetTypeInfo().Assembly);
            config.Build();

            return services.BuildServiceProvider();
        }
    }

    internal sealed class FirstTestEvent : BaseEvent
    {
        public FirstTestEvent(Guid entityId, long version = -2)
            : base(entityId, version)
        { }

        public override Task Log()
        {
            return Task.CompletedTask;
        }
    }

    internal sealed class SecondTestEvent : BaseEvent
    {
        public SecondTestEvent(Guid entityId, long version = -2)
            : base(entityId, version)
        { }

        public override Task Log()
        {
            return Task.CompletedTask;
        }
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