using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

using Erden.Cqrs;
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

            var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
            var assemblies = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId);
            var loadContext = AssemblyLoadContext.Default;

            foreach (var assemblyName in assemblies)
            {
                var assembly = loadContext.LoadFromAssemblyName(assemblyName);
                RegisterHandlers(assembly);
            }

            var registrator = new AutoRegistrator(services.BuildServiceProvider());
            registrator.Register(typeof(IEventHandler<>), typeof(IEventHandlerRegistrator), "Handle");
            registrator.Register(typeof(ICommandHandler<>), typeof(ICommandHandlerRegistrator), "Execute");
            registrator.Register(typeof(IQueryHandler<,>), typeof(IQueryHandlerRegistrator), "Execute");
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
                                && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICommandHandler<>)) ||
                            allInterfaces.Any(y =>
                                y.GetTypeInfo().IsGenericType
                                && y.GetTypeInfo().GetGenericTypeDefinition() == typeof(IQueryHandler<,>)) ||
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