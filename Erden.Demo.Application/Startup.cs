using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Erden.Cqrs;
using System.Reflection;
using Erden.EventSourcing;
using Erden.Domain;
using Erden.EventSourcing.Targets.EventStore;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            OptionsConfigurationServiceCollectionExtensions.Configure<EventStoreSettings>(services, Configuration.GetSection("EventStore"));
            services.AddMvc();

            new ESConfiguration(services)
                .UseDefaultEventBus()
                .WithAssembly(typeof(Startup).GetTypeInfo().Assembly)
                .Build();
            services.AddSingleton<IEventStore, EventStoreTarget>();

            new DomainConfiguration(services)
                .Build();

            var cqrs = new CqrsConfiguration(services)
                .UseDefaultCommandBus()
                .UseDefaultDataStorage()
                .WithAssembly(typeof(Startup).GetTypeInfo().Assembly);
            cqrs.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
