using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Erden.Configuration
{
    /// <summary>
    /// Erden configuration class
    /// </summary>
    public class ErdenConfig
    {
        /// <summary>
        /// List of meta information about used parts of Erden
        /// </summary>
        private readonly List<HandlerRegistrationInfo> meta = new List<HandlerRegistrationInfo>();

        /// <summary>
        /// Initialize a new instance of <see cref="ErdenConfig"/> class with <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services">Service collection for DI</param>
        public ErdenConfig(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Service collection
        /// </summary>
        public readonly IServiceCollection Services;

        /// <summary>
        /// Build config
        /// </summary>
        public void Build()
        {
            var registrator = new AutoRegistrator(Services);
            foreach (var info in meta)
            {
                registrator.AddHandlers(info.HandlerType);
                registrator.Register(info.HandlerType, info.RegistratorType, info.HandlerMethod);
            }
        }

        /// <summary>
        /// Add handlers for autoregistration
        /// </summary>
        /// <param name="handlerType">Handler type</param>
        /// <param name="registratorType">Registrator type</param>
        /// <param name="handlerMethod">Handler method</param>
        public void AddToRegistration(Type handlerType, Type registratorType, string handlerMethod)
        {
            meta.Add(new HandlerRegistrationInfo
            {
                HandlerType = handlerType,
                RegistratorType = registratorType,
                HandlerMethod = handlerMethod
            });
        }
    }
}