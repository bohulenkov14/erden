using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

using Erden.Cqrs.Core;

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
        /// <summary>
        /// Assemblies, where command and query handlers placed
        /// </summary>
        private readonly List<Assembly> assemblies = new List<Assembly>();

        public CqrsConfiguration(IServiceCollection services)
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
            registrator.Register(typeof(ICommandHandler<>), typeof(ICommandHandlerRegistrator));
            registrator.Register(typeof(IQueryHandler<,>), typeof(IQueryHandlerRegistrator));
        }

        /// <summary>
        /// Add assembly, where command and query handlers placed
        /// </summary>
        /// <param name="assembly">Assembly</param>
        public CqrsConfiguration WithAssembly(Assembly assembly)
        {
            assemblies.Add(assembly);
            return this;
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
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICommandHandler<>)) ||
                            allInterfaces.Any(y => y.GetTypeInfo().IsGenericType && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
                    }))
                    .AsSelf()
                    .WithTransientLifetime()
            );
        }
    }
}