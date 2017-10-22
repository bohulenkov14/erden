using Microsoft.Extensions.DependencyInjection;

using Erden.Core;
using Erden.Cqrs;
using Erden.Dal;
using Erden.Domain;
using Erden.EventSourcing;

namespace Erden.Configuration
{
    public sealed class ErdenConfig
    {
        private readonly IServiceCollection services;

        public ErdenConfig(IServiceCollection services)
        {
            this.services = services;
        }

        public void Build()
        {
            services.AddSingleton<IAggregateStorage, AggregateStorage>();
            services.AddSingleton<ISession, Session>();

            var registrator = new AutoRegistrator(services);
            registrator.AddHandlers(typeof(IEventHandler<>));
            registrator.AddHandlers(typeof(ICommandHandler<>));
            registrator.AddHandlers(typeof(IQueryHandler<,>));
            registrator.AddHandlers(typeof(IChangeHandler<>));
            registrator.AddHandlers(typeof(IFetchHandler<,>));
            registrator.Register(typeof(IEventHandler<>), typeof(IEventHandlerRegistrator), "Handle");
            registrator.Register(typeof(ICommandHandler<>), typeof(ICommandHandlerRegistrator), "Execute");
            registrator.Register(typeof(IQueryHandler<,>), typeof(IQueryHandlerRegistrator), "Execute");
            registrator.Register(typeof(IChangeHandler<>), typeof(IChangeHandlerRegistrator), "Execute");
            registrator.Register(typeof(IFetchHandler<,>), typeof(IFetchHandlerRegistrator), "Execute");
        }

        public ErdenConfig AddEventStoreTarget<T>() where T : class, IEventStore
        {
            services.AddSingleton<IEventStore, T>();
            return this;
        }

        public ErdenConfig UseDefaultEventBus()
        {
            var inMemoryEventBus = new InMemoryEventBus();
            services.AddSingleton<IEventHandlerRegistrator>(provider => inMemoryEventBus);
            services.AddSingleton<IEventPublisher>(provider => inMemoryEventBus);
            return this;
        }

        /// <summary>
        /// Use default in memory command bus
        /// </summary>
        public ErdenConfig UseDefaultCommandBus()
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
        public ErdenConfig UseDefaultDataStorage()
        {
            var inMemoryDataStorage = new InMemoryDataStorage();
            services.AddSingleton<IDataStorage>(provider => inMemoryDataStorage);
            services.AddSingleton<IQueryHandlerRegistrator>(provider => inMemoryDataStorage);
            return this;
        }

        public ErdenConfig UseDefaultFetchSource()
        {
            var inMemoryStorage = new InMemoryStorage();
            services.AddSingleton<IStorage>(provider => inMemoryStorage);
            services.AddSingleton<IFetchHandlerRegistrator>(provider => inMemoryStorage);
            return this;
        }

        public ErdenConfig UseDefaultChangeBus()
        {
            var inMemoryChangesBus = new InMemoryChangesBus();
            services.AddSingleton<IChangesBus>(provider => inMemoryChangesBus);
            services.AddSingleton<IChangeHandlerRegistrator>(provider => inMemoryChangesBus);
            return this;
        }
    }
}