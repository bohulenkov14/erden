using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

using Erden.Cqrs.Example.Application.Commands;
using Erden.Cqrs.Example.Application.Queries;

namespace Erden.Cqrs.Example.Application
{
    class Program
    {
        private static IServiceCollection services;

        static void Main(string[] args)
        {
            Console.WriteLine("Erden CQRS example");
            Console.WriteLine();
            services = new ServiceCollection();
            var cqrsConfiguration = new CqrsConfiguration(services)
                .UseDefaultCommandBus()
                .UseDefaultDataStorage();
            cqrsConfiguration.Build();

            var provider = services.BuildServiceProvider();
            ICommandBus bus = provider.GetService<ICommandBus>();
            IDataStorage storage = provider.GetService<IDataStorage>();

            bus.Send(new ExampleCommand()).Wait();
            var result = storage.Retrieve(new ExampleQuery()).Result;
            Console.WriteLine($"Query result is {result}");

            Console.WriteLine();
            Console.WriteLine("Press any button to exit");
            Console.ReadKey(false);
        }
    }
}
