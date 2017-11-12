﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Erden.Configuration;

namespace Erden.EventSourcing
{
    public static class ConfigExtensions
    {
        public static ErdenConfig AddEventSourcing(this ErdenConfig config)
        {
            config.AddToRegistration(typeof(IEventHandler<>), typeof(IEventHandlerRegistrator), "Handle");
            return config;
        }

        public static ErdenConfig AddEventStoreTarget<T>(this ErdenConfig config) where T : class, IEventStore
        {
            config.Services.AddSingleton<IEventStore, T>();
            return config;
        }

        public static ErdenConfig UseDefaultEventBus(this ErdenConfig config, ILogger logger)
        {
            var inMemoryEventBus = new InMemoryEventBus(logger);
            config.Services.AddSingleton<IEventHandlerRegistrator>(provider => inMemoryEventBus);
            config.Services.AddSingleton<IEventPublisher>(provider => inMemoryEventBus);
            return config;
        }
    }
}