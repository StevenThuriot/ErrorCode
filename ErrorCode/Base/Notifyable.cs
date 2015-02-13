#region License

//  
// Copyright 2015 Steven Thuriot
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

#endregion

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