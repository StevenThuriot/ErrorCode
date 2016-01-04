using ErrorCode.Base;
using ErrorCode.Domain;
using System.Windows.Input;

namespace ErrorCode.ViewModels.Commands
{
    class TestCommand : LoadingCommand<Overview>
    {
        public override bool CanExecute(object parameter) => !App.IsLoading && (parameter is TestAssembly || parameter is TestClass || parameter is Test);


        protected override void ExecuteWhileLoading(object parameter)
        {
            var commands = ViewModel.Commands;

            if (TryRun(commands.RunAllAssemblyTests, parameter))
                return;

            if (TryRun(commands.RunAllClassTests, parameter))
                return;

            TryRun(commands.RunTest, parameter);
        }

        bool TryRun(ICommand command, object parameter)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
                return true;
            }

            return false;
        }
    }
}
