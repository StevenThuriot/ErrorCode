using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace ErrorCode.Base
{
    public class CommandsManager<T> : DynamicObject
        where T : ViewModel<T>
    {
        private readonly ViewModel<T> _viewModel;
        private readonly Dictionary<string, Lazy<ICommand>> _commands;

        public CommandsManager(ViewModel<T> viewModel)
        {
            _viewModel = viewModel;

            var commands = Assembly.GetExecutingAssembly()
                                   .GetTypes()
                                   .Where(x => typeof (Command<T>).IsAssignableFrom(x))
                                   .Where(x => !x.IsAbstract)
                                   .ToList();

            _commands = new Dictionary<string, Lazy<ICommand>>(commands.Count, StringComparer.OrdinalIgnoreCase);
            
            foreach (var command in commands)
            {
                var copy = command;

                var name = copy.Name;
                var lazyCommand = new Lazy<ICommand>(() => Activate(copy));

                _commands.Add(name, lazyCommand);
            }
        }

        private ICommand Activate(Type type)
        {
            var command = (Command<T>) Activator.CreateInstance(type);
            command.Init((T) _viewModel);

            return command;
        }


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Lazy<ICommand> lazyCommand;
            var name = binder.Name;
            if (_commands.TryGetValue(name, out lazyCommand))
            {
                result = lazyCommand.Value;
                return true;
            }

            const string commandSuffix = "Command";

            if (name.EndsWith(commandSuffix, StringComparison.OrdinalIgnoreCase))
            {
                name = name.Substring(0, name.Length - commandSuffix.Length);
            }
            else
            {
                name += commandSuffix;
            }

            if (_commands.TryGetValue(name, out lazyCommand))
            {
                result = lazyCommand.Value;
                return true;
            }

            result = null;
            return false;
        }
    }
}