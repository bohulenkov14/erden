using System.Threading.Tasks;

using Erden.Cqrs.Example.Application.Queries;

namespace Erden.Cqrs.Example.Application.QueryHandlers
{
    internal sealed class ExampleQueryHandler : IQueryHandler<ExampleQuery, string>
    {
        public Task<string> Execute(ExampleQuery query)
        {
            return Task.Run(() => query.Id.ToString());
        }
    }
}