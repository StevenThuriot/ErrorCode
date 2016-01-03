using ErrorCode.Base;
using ErrorCode.Domain;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllClassTestsCommand : AsyncCommand<Overview>
    {
        public override bool CanExecute(object parameter) => !App.IsLoading && parameter is TestClass;

        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
            {
                using (App.Load)
                    ((TestClass)parameter).Run();

                return true;
            });
    }
}
