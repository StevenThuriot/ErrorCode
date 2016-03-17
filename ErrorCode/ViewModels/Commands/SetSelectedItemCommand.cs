using ErrorCode.Base;

namespace ErrorCode.ViewModels.Commands
{
    class SetSelectedItemCommand : Command<Overview>
    {
        public override void Execute(object parameter)
        {
            ViewModel.SelectedItem = parameter;
        }
    }
}
