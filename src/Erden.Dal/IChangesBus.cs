using System.Threading.Tasks;

namespace Erden.Dal
{
    public interface IChangesBus
    {
        Task Send<T>(T request) where T : IChangeRequest;
        Task Send<T>(params object[] args) where T : IChangeRequest;
    }
}