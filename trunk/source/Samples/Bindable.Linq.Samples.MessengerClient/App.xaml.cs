using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Bindable.Linq.Samples.MessengerClient.MessengerService;
using Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator;

namespace Bindable.Linq.Samples.MessengerClient
{
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
            MessengerWindow window = new MessengerWindow(service);
            this.MainWindow = window;
            window.Show();
        }
    }
}
