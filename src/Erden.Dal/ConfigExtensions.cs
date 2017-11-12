using Microsoft.Extensions.DependencyInjection;

using Erden.Configuration;

namespace Erden.Dal
{
    /// <summary>
    /// DAL config extensions
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Add DAL
        /// </summary>
        public static ErdenConfig AddDal(this ErdenConfig config)
        {
            config.AddToRegistration(typeof(IChangeHandler<>), typeof(IChangeHandlerRegistrator), "Execute");
            config.AddToRegistration(typeof(IFetchHandler<,>), typeof(IFetchHandlerRegistrator), "Execute");
            return config;
        }

        /// <summary>
        /// Use default fetch source - <see cref="InMemoryStorage"/>
        /// </summary>
        public static ErdenConfig UseDefaultFetchSource(this ErdenConfig config)
        {
            var inMemoryStorage = new InMemoryStorage();
            config.Services.AddSingleton<IStorage>(provider => inMemoryStorage);
            config.Services.AddSingleton<IFetchHandlerRegistrator>(provider => inMemoryStorage);
            return config;
        }

        /// <summary>
        /// Use default changes bus - <see cref="InMemoryChangesBus"/>
        /// </summary>
        public static ErdenConfig UseDefaultChangeBus(this ErdenConfig config)
        {
            var inMemoryChangesBus = new InMemoryChangesBus();
            config.Services.AddSingleton<IChangesBus>(provider => inMemoryChangesBus);
            config.Services.AddSingleton<IChangeHandlerRegistrator>(provider => inMemoryChangesBus);
            return config;
        }
    }
}