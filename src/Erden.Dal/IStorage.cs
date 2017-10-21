using System.Threading.Tasks;

namespace Erden.Dal
{
    public interface IStorage
    {
        Task<TResult> Retrieve<TResult>(IFetchRequest<TResult> request) where TResult : class;
    }
}