using Microsoft.Extensions.DependencyInjection;

using Erden.Core;

namespace Erden.Cqrs
{
    /// <summary>
    /// Configuration for CQRS realization
    /// </summary>
    public sealed class CqrsConfiguration
    {
        /// <summary>
        /// DI container
        /// </summary>
        private readonly IServiceCollection services;

        public CqrsConfiguration(IServiceCollection services)
        {
            this.services = services;
        }

        /// <summary>
        /// Build configuration
        /// </summary>
        public void Build()
        {
            var registrator = new AutoRegistrator(services);
            registrator.AddHandlers(typeof(ICommandHandler<>));
            registrator.AddHandlers(typeof(IQueryHandler<,>));

            registrator.Register(typeof(ICommandHandler<>), typeof(ICommandHandlerRegistrator), "Execute");
            registrator.Register(typeof(IQueryHandler<,>), typeof(IQueryHandlerRegistrator), "Execute");
        }

        /// <summary>
        /// Use default in memory command bus
        /// </summary>
        public CqrsConfiguration UseDefaultCommandBus()
        {
            var inMemoryCommandBus = new InMemoryCommandBus();
            services.AddSingleton<ICommandHandlerRegistrator>(provider => inMemoryCommandBus);
            services.AddSingleton<ICommandBus>(provider => inMemoryCommandBus);
            return this;
        }

        /// <summary>
        /// Use default in memory data storage
        /// </summary>
        /// <returns></returns>
        public CqrsConfiguration UseDefaultDataStorage()
        {
            var inMemoryDataStorage = new InMemoryDataStorage();
            services.AddSingleton<IDataStorage>(provider => inMemoryDataStorage);
            services.AddSingleton<IQueryHandlerRegistrator>(provider => inMemoryDataStorage);
            return this;
        }
    }
}