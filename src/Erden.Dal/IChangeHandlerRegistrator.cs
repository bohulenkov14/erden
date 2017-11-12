using System;
using System.Threading.Tasks;

namespace Erden.Dal
{
    /// <summary>
    /// Interface for change handler registrator
    /// </summary>
    public interface IChangeHandlerRegistrator
    {
        /// <summary>
        /// Register handler
        /// </summary>
        /// <typeparam name="T">Change request type</typeparam>
        /// <param name="handler">Handler</param>
        void Register<T>(Func<T, Task> handler) where T : IChangeRequest;
    }
}