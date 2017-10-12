using System;
using System.Threading.Tasks;

namespace Erden.Cqrs.Example.Application.Commands
{
    internal sealed class ExampleCommand : BaseCommand
    {
        public override Task Log()
        {
            Console.WriteLine($"Executed ExampleCommand with id {Id}");
            return Task.CompletedTask;
        }
    }
}