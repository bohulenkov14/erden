using System;
using System.Threading.Tasks;

using Erden.Cqrs.Example.Application.Commands;

namespace Erden.Cqrs.Example.Application.CommandHandlers
{
    internal sealed class ExampleCommandHandler : ICommandHandler<ExampleCommand>
    {
        public Task Execute(ExampleCommand command)
        {
            Console.WriteLine($"Command id {command.Id}");
            return Task.CompletedTask;
        }
    }
}