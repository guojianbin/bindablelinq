namespace Bindable.Linq.SampleApplication
{
    using System.Windows;
    using Samples;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Window w = new SyncLinqFilteredWindow();
            MainWindow = w;
            w.Show();
        }
    }
}