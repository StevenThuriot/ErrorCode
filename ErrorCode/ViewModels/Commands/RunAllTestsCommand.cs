using ErrorCode.Base;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllTestsCommand : LoadingAsyncCommand<Overview>
    {
        public override bool CanExecute(object parameter) => !ViewModel.IsLoading;

        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
                    {
                        foreach (var test in ViewModel.Tests)
                            test.Run();

                        return true;
                    });
    }
}
