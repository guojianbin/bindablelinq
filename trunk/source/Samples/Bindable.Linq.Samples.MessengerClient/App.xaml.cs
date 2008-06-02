namespace Bindable.Linq.Samples.MessengerClient
{
    using System.Windows;
    using Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator;
    using MessengerService;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IMessengerService service = new SimulationMessengerService();
            var window = new MessengerWindow(service);
            MainWindow = window;
            window.Show();
        }
    }
}