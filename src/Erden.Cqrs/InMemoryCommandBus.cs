using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erden.Cqrs
{
    public class InMemoryCommandBus : ICommandBus, ICommandHandlerRegistrator
    {
        private readonly Dictionary<Type, Func<ICommand, Task>> handlers
            = new Dictionary<Type, Func<ICommand, Task>>();

        public void Register<T>(Func<T, Task> handler) where T : ICommand
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            handlers.Add(typeof(T), x => handler((T)x));
        }

        public async Task Send<T>(T command) where T : ICommand
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (!handlers.TryGetValue(typeof(T), out var handler))
                throw new Exception();

            await command.Log();
            await handler.Invoke(command);
        }
    }
}