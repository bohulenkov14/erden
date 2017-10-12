using System;
using System.Threading.Tasks;

namespace Erden.Cqrs.Example.Application.Queries
{
    internal sealed class ExampleQuery : BaseQuery<string>
    {
        public override Task Log()
        {
            Console.WriteLine($"Executed ExampleQuery with id {Id}");
            return Task.CompletedTask;
        }
    }
}