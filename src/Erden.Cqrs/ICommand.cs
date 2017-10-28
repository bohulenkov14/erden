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
        /// Call timestamp
        /// </summary>
        long Timestamp { get; }
    }
}