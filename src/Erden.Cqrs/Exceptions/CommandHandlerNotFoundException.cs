using System;

namespace Erden.Cqrs.Exceptions
{
    /// <summary>
    /// The exception that is thrown when command handler not found
    /// </summary>
    public class CommandHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="CommandHandlerNotFoundException"/> class with the type of command which handler not found
        /// </summary>
        /// <param name="commandType">Command type</param>
        public CommandHandlerNotFoundException(Type commandType)
            : base($"Handler for command with type {commandType.Name} not found")
        { }
    }
}