using ErrorCode.Base;
using ErrorCode.Domain;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunTestCommand : TypedLoadingAsyncCommand<Overview, Test>
    {
        protected override bool CanExecute(Test parameter) => !ViewModel.IsLoading;

        protected override Task<bool> OnExecute(Test parameter) =>
            Task.Run(() =>
            {
                var instance = parameter.Parent.CreateTestInstance();
                parameter.Run(instance);

                return true;
            });
    }
}
