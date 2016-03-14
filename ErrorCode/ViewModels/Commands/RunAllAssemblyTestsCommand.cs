using ErrorCode.Base;
using ErrorCode.Domain;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllAssemblyTestsCommand : TypedLoadingAsyncCommand<Overview, TestAssembly>
    {
        protected override bool CanExecute(TestAssembly parameter) => !ViewModel.IsLoading;

        protected override Task<bool> OnExecute(TestAssembly parameter) =>
            Task.Run(() =>
            {
                foreach (var testClass in parameter)
                    testClass.Run();

                return true;
            });
    }
}
