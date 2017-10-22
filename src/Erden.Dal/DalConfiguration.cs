using Microsoft.Extensions.DependencyInjection;

using Erden.Core;

namespace Erden.Dal
{
    public class DalConfiguration
    {
        private readonly IServiceCollection services;

        public DalConfiguration(IServiceCollection services)
        {
            this.services = services;
        }

        public void Build()
        {
            var registrator = new AutoRegistrator(services);

            registrator.AddHandlers(typeof(IChangeHandler<>));
            registrator.AddHandlers(typeof(IFetchHandler<,>));

            registrator.Register(typeof(IChangeHandler<>), typeof(IChangeHandlerRegistrator), "Execute");
            registrator.Register(typeof(IFetchHandler<,>), typeof(IFetchHandlerRegistrator), "Execute");
        }

        public DalConfiguration UseDefaultChangesBus()
        {
            var inMemoryChangesBus = new InMemoryChangesBus();
            services.AddSingleton<IChangeHandlerRegistrator>(provider => inMemoryChangesBus);
            services.AddSingleton<IChangesBus>(provider => inMemoryChangesBus);
            return this;
        }

        public DalConfiguration UseDefaultStorage()
        {
            var inMemoryStorage = new InMemoryStorage();
            services.AddSingleton<IFetchHandlerRegistrator>(provider => inMemoryStorage);
            services.AddSingleton<IStorage>(provider => inMemoryStorage);
            return this;
        }
    }
}