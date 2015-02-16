#region License

//  
// Copyright 2015 Steven Thuriot
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

#endregion

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