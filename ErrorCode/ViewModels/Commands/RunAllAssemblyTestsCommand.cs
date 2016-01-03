using ErrorCode.Base;
using ErrorCode.Domain;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllAssemblyTestsCommand : AsyncCommand<Overview>
    {
        public override bool CanExecute(object parameter) => !App.IsLoading && parameter is TestAssembly;

        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
            {
                using (App.Load)
                foreach (var testClass in (TestAssembly)parameter)
                    testClass.Run();

                return true;
            });
    }
}
