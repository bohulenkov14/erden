using System.Threading.Tasks;

namespace Erden.Dal
{
    public interface IStorage
    {
        Task<TResult> Retrieve<TResult>(IFetchRequest<TResult> request) where TResult : class;
        Task<TResult> Retrieve<T, TResult>(params object[] args)
            where TResult : class
            where T : IFetchRequest<TResult>;
    }
}