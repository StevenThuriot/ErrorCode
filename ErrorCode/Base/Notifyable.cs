using System.Collections.Generic;
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

        protected bool ChangeProperty<T>(ref T orignal, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(orignal, value)) return false;

            orignal = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}