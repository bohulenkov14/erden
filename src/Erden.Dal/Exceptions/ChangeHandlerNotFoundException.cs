using System;

namespace Erden.Dal.Exceptions
{
    /// <summary>
    /// The exception that is thrown when change handler not found
    /// </summary>
    public class ChangeHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ChangeHandlerNotFoundException"/> class with the type of handler which was not found
        /// </summary>
        /// <param name="handlerType">Handler type</param>
        public ChangeHandlerNotFoundException(Type handlerType)
            : base($"Change hanlder with type {handlerType.Name} not found")
        { }
    }
}