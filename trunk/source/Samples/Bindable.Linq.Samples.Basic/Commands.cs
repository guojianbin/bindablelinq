using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Bindable.Linq.SampleApplication
{
    public static class Commands
    {
        public static RoutedUICommand Delete = new RoutedUICommand();
        public static RoutedUICommand Add = new RoutedUICommand();
        public static RoutedUICommand Refresh = new RoutedUICommand();
    }
}
