using ErrorCode.Base;
using ErrorCode.Domain;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunTestCommand : LoadingAsyncCommand<Overview>
    {
        public override bool CanExecute(object parameter) => !App.IsLoading && parameter is Test;

        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
            {
                var test = ((Test)parameter);

                var instance = test.Parent.CreateTestInstance();

                test.Run(instance);

                return true;
            });
    }
}
