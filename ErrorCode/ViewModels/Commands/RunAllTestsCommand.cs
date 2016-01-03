using ErrorCode.Base;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllTestsCommand : AsyncCommand<Overview>
    {
        public override bool CanExecute(object parameter) => !App.IsLoading;

        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
                    {
                        using (App.Load)
                        foreach (var test in ViewModel.Tests)
                        {
                            test.Run();
                        }

                        return true;
                    });
    }
}
