using System.Windows;

namespace ErrorCode.Base
{
    public interface IViewModel
    {
        dynamic Commands { get; }
        bool IsLoading { get; set; }
        Visibility IsLoadingVisibility { get; }
    }
}
