using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Erden.Cqrs.Exceptions;

namespace Erden.Cqrs
{
    /// <summary>
    /// Default realization of <see cref="ICommandBus"/> and <see cref="ICommandHandlerRegistrator"/>
    /// Provides command handlers registration and command execution
    /// </summary>
    public class InMemoryCommandBus : ICommandBus, ICommandHandlerRegistrator
    {
        /// <summary>
        /// Command handlers
        /// </summary>
        private readonly Dictionary<Type, Func<ICommand, Task>> handlers
            = new Dictionary<Type, Func<ICommand, Task>>();

        /// <summary>
        /// Register command handler
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="handler">Command handler</param>
        public void Register<T>(Func<T, Task> handler) where T : ICommand
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            handlers.Add(typeof(T), x => handler((T)x));
        }

        /// <summary>
        /// Send command to handler
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="command">Command</param>
        public async Task Send<T>(T command) where T : ICommand
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (!handlers.TryGetValue(typeof(T), out var handler))
                throw new CommandHandlerNotFoundException(typeof(T));

            await handler.Invoke(command);
        }
        /// <summary>
        /// Send command to handler
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        public Task Send<T>(params object[] args) where T : ICommand
        {
            return Send((T)Activator.CreateInstance(typeof(T), args));
        }
        /// <summary>
        /// Send command to handler
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="args">Command's constructor args</param>
        public Task Send<T>() where T : ICommand
        {
            return Send((T)Activator.CreateInstance<T>());
        }
    }
}