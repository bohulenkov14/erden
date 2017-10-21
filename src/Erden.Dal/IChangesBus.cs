using System.Threading.Tasks;

namespace Erden.Dal
{
    public interface IChangesBus
    {
        Task Send<T>(T request) where T : IChangeRequest;
    }
}