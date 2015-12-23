using ErrorCode.Base;
using ErrorCode.Domain;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunAllClassTestsCommand : AsyncCommand<Overview>
    {
        public override bool CanExecute(object parameter) => parameter is TestClass;

        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
            {
                ((TestClass)parameter).Run();
                return true;
            });
    }
}
