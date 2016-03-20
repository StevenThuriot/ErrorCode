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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            var @event = PropertyChanged;
            if (@event != null)
            {
                foreach (var propertyName in propertyNames)
                    @event.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetValue<T>(ref T orignal, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(orignal, value))
                return false;

            orignal = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}