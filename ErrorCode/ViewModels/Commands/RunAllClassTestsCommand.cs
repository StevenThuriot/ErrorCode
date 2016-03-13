using ErrorCode.Base;
using ErrorCode.Domain;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllClassTestsCommand : TypedLoadingAsyncCommand<Overview, TestClass>
    {
        protected override bool CanExecute(TestClass parameter) => !App.IsLoading;

        protected override Task<bool> OnExecute(TestClass parameter) =>
            Task.Run(() =>
            {
                parameter.Run();
                return true;
            });
    }
}
