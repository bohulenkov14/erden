using Microsoft.Extensions.DependencyInjection;

using Erden.Configuration;

namespace Erden.Domain
{
    public static class ConfigExtensions
    {
        public static ErdenConfig AddDomain(this ErdenConfig config)
        {
            config.Services.AddSingleton<IAggregateStorage, AggregateStorage>();
            config.Services.AddSingleton<ISession, Session>();
            return config;
        }
    }
}