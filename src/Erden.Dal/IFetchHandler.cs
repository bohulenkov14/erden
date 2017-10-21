using System.Threading.Tasks;

namespace Erden.Dal
{
    public interface IFetchHandler<TFetch, TResult>
        where TFetch : IFetchRequest<TResult>
        where TResult : class
    {
        Task<TResult> Execute(TFetch request);
    }
}