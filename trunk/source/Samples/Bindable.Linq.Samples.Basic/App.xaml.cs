using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Bindable.Linq.SampleApplication.Samples;

namespace Bindable.Linq.SampleApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Window w = new SyncLinqFilteredWindow();
            this.MainWindow = w;
            w.Show();
        }
    }
}
