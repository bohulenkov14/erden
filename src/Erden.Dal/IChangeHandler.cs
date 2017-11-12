using System.Threading.Tasks;

namespace Erden.Dal
{
    /// <summary>
    /// Change handler interface
    /// </summary>
    /// <typeparam name="T">Change request type</typeparam>
    public interface IChangeHandler<T> where T : IChangeRequest
    {
        /// <summary>
        /// Execute change request
        /// </summary>
        /// <param name="request">Change request</param>
        Task Execute(T request);
    }
}