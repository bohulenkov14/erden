using System;

namespace Erden.Dal.Exceptions
{
    /// <summary>
    /// The exception that is thrown when fetch handler not found
    /// </summary>
    public class FetchHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="FetchHandlerNotFoundException"/> class with the type of handler which was not found
        /// </summary>
        /// <param name="handlerType">Handler type</param>
        public FetchHandlerNotFoundException(Type handlerType)
            : base($"Fetch hanlder with type {handlerType.Name} not found")
        { }
    }
}