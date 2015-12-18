using ErrorCode.Base;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllTestsCommand : AsyncCommand<Overview>
    {
        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
                    {
                        foreach (var test in ViewModel.Tests)
                        {
                            test.Run();
                        }

                        return true;
                    });
    }
}
