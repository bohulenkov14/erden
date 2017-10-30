using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

using Erden.Configuration;

namespace Erden.Cqrs.Tests
{
    public sealed class InMemoryCommandBusTest
    {
        [Fact]
        public async Task Command_Should_Be_Executed_Test()
        {
            var provider = Configure();

            ICommandBus bus = provider.GetService<ICommandBus>();

            var entity = new TestEntity { TestProperty = 0 };
            await bus.Send(new FirstTestCommand(entity));
            Assert.Equal(1, entity.TestProperty);
        }

        [Fact]
        public async Task Command_Should_Be_Executed_Multiple_Time_Test()
        {
            var provider = Configure();

            ICommandBus bus = provider.GetService<ICommandBus>();

            var entity = new TestEntity { TestProperty = -1 };

            for (int i = 0; i < 3; ++i)
            {
                await bus.Send(new FirstTestCommand(entity));
                Assert.Equal(i, entity.TestProperty);
            }
        }

        [Fact]
        public async Task Different_Commands_Should_Be_Executed_Test()
        {
            var provider = Configure();

            ICommandBus bus = provider.GetService<ICommandBus>();

            var entity = new TestEntity { TestProperty = 0 };
            await bus.Send(new FirstTestCommand(entity));
            Assert.Equal(1, entity.TestProperty);
            await bus.Send(new SecondTestCommand(entity));
            Assert.Equal(2, entity.TestProperty);
        }

        private ServiceProvider Configure()
        {
            var services = new ServiceCollection();
            var config = new ErdenConfig(services)
                .AddCqrs()
                .UseDefaultCommandBus()
                .UseDefaultDataStorage();
            config.Build();

            return services.BuildServiceProvider();
        }
    }

    internal sealed class TestEntity
    {
        public int TestProperty { get; set; }
    }

    internal sealed class FirstTestCommand : BaseCommand
    {
        public FirstTestCommand(TestEntity entity)
        {
            Entity = entity;
        }

        public TestEntity Entity { get; private set; }
    }

    internal sealed class SecondTestCommand : BaseCommand
    {
        public SecondTestCommand(TestEntity entity)
        {
            Entity = entity;
        }

        public TestEntity Entity { get; private set; }
    }

    internal sealed class FirstTestCommandHandler : ICommandHandler<FirstTestCommand>
    {
        public Task Execute(FirstTestCommand command)
        {
            command.Entity.TestProperty++;
            return Task.CompletedTask;
        }
    }

    internal sealed class SecondTestCommandHandler : ICommandHandler<SecondTestCommand>
    {
        public Task Execute(SecondTestCommand command)
        {
            command.Entity.TestProperty++;
            return Task.CompletedTask;
        }
    }
}