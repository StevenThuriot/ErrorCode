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

using System.Windows;
using ErrorCode.ViewModels;

namespace ErrorCode
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty CurrentViewModelProperty =
            DependencyProperty.Register("CurrentViewModel", typeof (object), typeof (MainWindow),
                                        new PropertyMetadata(new Overview()));


        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
                                                                                                  "IsLoading",
                                                                                                  typeof (bool),
                                                                                                  typeof (MainWindow),
                                                                                                  new PropertyMetadata(
                                                                                                      false,
                                                                                                      OnIsLoadingChanged));


        public static readonly DependencyPropertyKey IsLoadingVisibilityPropertyKey = DependencyProperty
            .RegisterReadOnly(
                              "IsLoadingVisibility", typeof (Visibility), typeof (MainWindow),
                              new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty IsLoadingVisibilityProperty =
            IsLoadingVisibilityPropertyKey.DependencyProperty;

        public MainWindow()
        {
            InitializeComponent();
        }

        public object CurrentViewModel
        {
            get { return GetValue(CurrentViewModelProperty); }
            set { SetValue(CurrentViewModelProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public Visibility IsLoadingVisibility
        {
            get { return (Visibility) GetValue(IsLoadingVisibilityProperty); }
            protected set { SetValue(IsLoadingVisibilityPropertyKey, value); }
        }


        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MainWindow) d).IsLoadingVisibility =
                (bool) e.NewValue
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }
    }
}