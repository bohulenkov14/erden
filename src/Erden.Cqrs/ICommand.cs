using System;
using System.Threading.Tasks;

namespace Erden.Cqrs
{
    /// <summary>
    /// Command interface
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Command ID
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Command timestamp
        /// </summary>
        long Timestamp { get; }

        /// <summary>
        /// Log command
        /// </summary>
        Task Log();
    }
}