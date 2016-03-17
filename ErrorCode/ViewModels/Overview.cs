using System.Collections.Generic;
using ErrorCode.Base;
using ErrorCode.Domain;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace ErrorCode.ViewModels
{
    class Overview : ViewModel<Overview>
    {
        public Overview()
        {
            App.LeftWindowControls.Clear();
            App.RightWindowControls.Clear();

            var runAllTestsButton = new Button
            {
                Content = new Path
                {
                    Fill = Brushes.White,
                    Data = Geometry.Parse("M 6 0 L 6 12 L 12 6 Z")
                },
                Command = Commands.RunAllTests,
                ToolTip = @"Run All Tests"
            };

            App.RightWindowControls.Add(runAllTestsButton);

            Tests = Discover.Tests();
        }

        public IEnumerable<TestAssembly> Tests { get; }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { SetValue(ref _selectedItem, value); }
        }
    }
}