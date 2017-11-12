using System.Threading.Tasks;

namespace Erden.Dal
{
    /// <summary>
    /// Changes bus interface
    /// </summary>
    public interface IChangesBus
    {
        /// <summary>
        /// Send request for execution
        /// </summary>
        /// <typeparam name="T">Change request type</typeparam>
        /// <param name="request">Change request</param>
        Task Send<T>(T request) where T : IChangeRequest;
        /// <summary>
        /// Send request for execution
        /// </summary>
        /// <typeparam name="T">Change request type</typeparam>
        Task Send<T>() where T : IChangeRequest;
        /// <summary>
        /// Send request for execution
        /// </summary>
        /// <typeparam name="T">Change request type</typeparam>
        /// <param name="args">Args to create change request</param>
        Task Send<T>(params object[] args) where T : IChangeRequest;
    }
}