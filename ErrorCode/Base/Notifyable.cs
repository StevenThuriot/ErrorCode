using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ErrorCode.Base
{
	public class Notifyable : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
		    var handler = PropertyChanged;
		    if (handler == null) return;
		    handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	    protected void ChangeProperty<T>(ref T orignal, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(orignal, value)) return;

            orignal = value;
            OnPropertyChanged(propertyName);
		}
	}
}
