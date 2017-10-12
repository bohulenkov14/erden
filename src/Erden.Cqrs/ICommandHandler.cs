using System.Threading.Tasks;

namespace Erden.Cqrs
{
    /// <summary>
    /// Commmand handler interface
    /// </summary>
    /// <typeparam name="T">Command type</typeparam>
    public interface ICommandHandler<T> where T : ICommand
    {
        /// <summary>
        /// Processes the command
        /// </summary>
        /// <param name="command">Command to process</param>
        Task Execute(T command);
    }
}