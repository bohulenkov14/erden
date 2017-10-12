using System;
using System.Threading.Tasks;

namespace Erden.Cqrs
{
    /// <summary>
    /// Command handler registrar
    /// </summary>
    public interface ICommandHandlerRegistrator
    {
        /// <summary>
        /// Register command handler
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="handler">Command handler</param>
        void Register<T>(Func<T, Task> handler) where T : ICommand;
    }
}