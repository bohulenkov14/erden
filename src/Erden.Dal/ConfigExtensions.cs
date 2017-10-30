using Microsoft.Extensions.DependencyInjection;

using Erden.Configuration;

namespace Erden.Dal
{
    public static class ConfigExtensions
    {
        public static ErdenConfig AddDal(this ErdenConfig config)
        {
            config.AddToRegistration(typeof(IChangeHandler<>), typeof(IChangeHandlerRegistrator), "Execute");
            config.AddToRegistration(typeof(IFetchHandler<,>), typeof(IFetchHandlerRegistrator), "Execute");
            return config;
        }

        public static ErdenConfig UseDefaultFetchSource(this ErdenConfig config)
        {
            var inMemoryStorage = new InMemoryStorage();
            config.Services.AddSingleton<IStorage>(provider => inMemoryStorage);
            config.Services.AddSingleton<IFetchHandlerRegistrator>(provider => inMemoryStorage);
            return config;
        }

        public static ErdenConfig UseDefaultChangeBus(this ErdenConfig config)
        {
            var inMemoryChangesBus = new InMemoryChangesBus();
            config.Services.AddSingleton<IChangesBus>(provider => inMemoryChangesBus);
            config.Services.AddSingleton<IChangeHandlerRegistrator>(provider => inMemoryChangesBus);
            return config;
        }
    }
}