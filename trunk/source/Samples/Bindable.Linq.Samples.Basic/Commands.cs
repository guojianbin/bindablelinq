namespace Bindable.Linq.SampleApplication
{
    using System.Windows.Input;

    public static class Commands
    {
        public static RoutedUICommand Add = new RoutedUICommand();
        public static RoutedUICommand Delete = new RoutedUICommand();
        public static RoutedUICommand Refresh = new RoutedUICommand();
    }
}