using ErrorCode.Base;
using System.Threading.Tasks;
using System.Linq;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllTestsCommand : LoadingAsyncCommand<Overview>
    {
        public override bool CanExecute(object parameter) => !ViewModel.IsLoading && ViewModel.Tests != null && ViewModel.Tests.Any();

        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
                    {
                        foreach (var test in ViewModel.Tests)
                            test.Run();


                        if (ViewModel.Tests.All(x => !x.IsSelected))
                            ViewModel.Tests.First().IsSelected = true;

                        return true;
                    });
    }
}
