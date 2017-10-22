﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Erden.EventSourcing.Targets.EventStore;
using Erden.Configuration;

namespace Erden.Demo.Application
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            OptionsConfigurationServiceCollectionExtensions.Configure<EventStoreSettings>(services, Configuration.GetSection("EventStore"));
            services.AddMvc();

            var erden = new ErdenConfig(services)
                .UseDefaultCommandBus()
                .UseDefaultDataStorage()
                .UseDefaultEventBus()
                .AddEventStoreTarget<EventStoreTarget>();
            erden.Build();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
