using System.Threading.Tasks;

namespace Erden.Cqrs
{
    /// <summary>
    /// Commands bus interface
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// Send command to handler
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="command">Command</param>
        Task Send<T>(T command) where T : ICommand;
        /// <summary>
        /// Send command to handler
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        Task Send<T>() where T : ICommand;
        /// <summary>
        /// Send command to handler
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="args">Command's constructor args</param>
        Task Send<T>(params object[] args) where T : ICommand;
    }
}
