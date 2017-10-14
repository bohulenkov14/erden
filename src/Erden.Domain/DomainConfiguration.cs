using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erden.Domain
{
    public class DomainConfiguration
    {
        private readonly IServiceCollection services;

        public DomainConfiguration(IServiceCollection services)
        {
            this.services = services;
        }

        public void Build()
        {
            services.AddSingleton<IAggregateStorage, AggregateStorage>();
            services.AddSingleton<ISession, Session>();
        }
    }
}