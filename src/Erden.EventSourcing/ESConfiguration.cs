using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

using Erden.EventSourcing.Core;

namespace Erden.EventSourcing
{
    public sealed class ESConfiguration
    {
        /// <summary>
        /// DI container
        /// </summary>
        private readonly IServiceCollection services;
        /// <summary>
        /// Assemblies, where command and query handlers placed
        /// </summary>
        private readonly List<Assembly> assemblies = new List<Assembly>();

        public ESConfiguration(IServiceCollection services)
        {
            this.services = services;
        }

        /// <summary>
        /// Build configuration
        /// </summary>
        public void Build()
        {
            foreach (var assembly in assemblies)
            {
                RegisterHandlers(assembly);
            }

            var registrator = new AutoRegistrator(services.BuildServiceProvider());
            registrator.Register(typeof(IEventHandler<>), typeof(IEventHandlerRegistrator));
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

        public ESConfiguration WithAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Register command and query handlers
        /// </summary>
        /// <param name="assembly">Assembly for registration</param>
        private void RegisterHandlers(Assembly assembly)
        {
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                    .AddClasses(classes => classes.Where(x => {
                        var allInterfaces = x.GetInterfaces();
                        return
                            allInterfaces.Any(y => 
                                y.GetTypeInfo().IsGenericType
                                && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IEventHandler<>));
                    }))
                    .AsSelf()
                    .WithTransientLifetime()
            );
        }
    }
}