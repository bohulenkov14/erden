using System.Threading.Tasks;

namespace Erden.Dal
{
    public interface IChangeHandler<T> where T : IChangeRequest
    {
        Task Execute(T request);
    }
}