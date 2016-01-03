using ErrorCode.Base;
using ErrorCode.Domain;
using System.Threading.Tasks;

namespace ErrorCode.ViewModels.Commands
{
    class RunTestCommand : AsyncCommand<Overview>
    {
        public override bool CanExecute(object parameter) => !App.IsLoading && parameter is Test;

        protected override Task<bool> OnExecute(object parameter) =>
            Task.Run(() =>
            {
                using (App.Load)
                {
                    var test = ((Test)parameter);

                    var instance = test.Parent.CreateTestInstance();

                    test.Run(instance);

                    return true;
                }
            });
    }
}
