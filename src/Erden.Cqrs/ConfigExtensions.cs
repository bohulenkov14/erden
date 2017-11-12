using Microsoft.Extensions.DependencyInjection;

using Erden.Configuration;

namespace Erden.Cqrs
{
    /// <summary>
    /// CQRS config extencions
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Add CQRS
        /// </summary>
        public static ErdenConfig AddCqrs(this ErdenConfig config)
        {
            config.AddToRegistration(typeof(ICommandHandler<>), typeof(ICommandHandlerRegistrator), "Execute");
            config.AddToRegistration(typeof(IQueryHandler<,>), typeof(IQueryHandlerRegistrator), "Execute");
            return config;
        }

        // <summary>
        /// Use default in memory command bus
        /// </summary>
        public static ErdenConfig UseDefaultCommandBus(this ErdenConfig config)
        {
            var inMemoryCommandBus = new InMemoryCommandBus();
            config.Services.AddSingleton<ICommandHandlerRegistrator>(provider => inMemoryCommandBus);
            config.Services.AddSingleton<ICommandBus>(provider => inMemoryCommandBus);
            return config;
        }

        /// <summary>
        /// Use default in memory data storage
        /// </summary>
        /// <returns></returns>
        public static ErdenConfig UseDefaultDataStorage(this ErdenConfig config)
        {
            var inMemoryDataStorage = new InMemoryDataStorage();
            config.Services.AddSingleton<IDataStorage>(provider => inMemoryDataStorage);
            config.Services.AddSingleton<IQueryHandlerRegistrator>(provider => inMemoryDataStorage);
            return config;
        }
    }
}