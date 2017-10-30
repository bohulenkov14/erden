using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

using Erden.Configuration;

namespace Erden.Cqrs.Tests
{
    public sealed class InMemoryDataStorageTest
    {
        public const string LOREM = "Lorem";
        public const string IPSUM = "Ipsum";

        [Fact]
        public async Task Query_Should_Be_Executed_Test()
        {
            var provider = Configure();

            IDataStorage storage = provider.GetService<IDataStorage>();

            var result = await storage.Retrieve(new FirstTestQuery());
            Assert.Equal("Lorem", result);
        }

        [Fact]
        public async Task Query_Should_Be_Executed_Multiple_Time_Test()
        {
            var provider = Configure();

            IDataStorage storage = provider.GetService<IDataStorage>();

            for (int i = 0; i < 3; ++i)
            {
                var result = await storage.Retrieve(new FirstTestQuery());
                Assert.Equal(LOREM, result);
            }
        }

        [Fact]
        public async Task Different_Queries_Should_Be_Executed_Test()
        {
            var provider = Configure();

            IDataStorage storage = provider.GetService<IDataStorage>();

            var firstResult = await storage.Retrieve(new FirstTestQuery());
            Assert.Equal(LOREM, firstResult);
            var secondResult = await storage.Retrieve(new SecondTestQuery());
            Assert.Equal(IPSUM, secondResult);
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

    internal sealed class FirstTestQuery : BaseQuery<string> { }

    internal sealed class SecondTestQuery : BaseQuery<string> { }

    internal sealed class FirstTestQueryHandler : IQueryHandler<FirstTestQuery, string>
    {
        public Task<string> Execute(FirstTestQuery query)
        {
            return Task.Run(() => InMemoryDataStorageTest.LOREM);
        }
    }

    internal sealed class SecondTestQueryHandler : IQueryHandler<SecondTestQuery, string>
    {
        public Task<string> Execute(SecondTestQuery query)
        {
            return Task.Run(() => InMemoryDataStorageTest.IPSUM);
        }
    }
}