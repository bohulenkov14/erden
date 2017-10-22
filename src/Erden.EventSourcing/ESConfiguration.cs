using Microsoft.Extensions.DependencyInjection;

using Erden.Core;

namespace Erden.EventSourcing
{
    public sealed class ESConfiguration
    {
        /// <summary>
        /// DI container
        /// </summary>
        private readonly IServiceCollection services;

        public ESConfiguration(IServiceCollection services)
        {
            this.services = services;
        }

        /// <summary>
        /// Build configuration
        /// </summary>
        public void Build()
        {
            var registrator = new AutoRegistrator(services);
            registrator.AddHandlers(typeof(IEventHandler<>));
            registrator.Register(typeof(IEventHandler<>), typeof(IEventHandlerRegistrator), "Handle");
        }

        public ESConfiguration UseDefaultEventBus()
        {
            var inMemoryEventBus = new InMemoryEventBus();
            services.AddSingleton<IEventHandlerRegistrator>(provider => inMemoryEventBus);
            services.AddSingleton<IEventPublisher>(provider => inMemoryEventBus);
            return this;
        }

        public ESConfiguration UseTestEventStore()
        {
            services.AddSingleton<IEventStore, InMemoryEventStore>();
            return this;
        }
    }
}