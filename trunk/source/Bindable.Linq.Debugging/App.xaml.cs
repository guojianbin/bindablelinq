using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;

namespace BindingOriented.SyncLinq.Debugging
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ObservableCollection<string> _strings = new ObservableCollection<string>();
            object query = _strings.AsBindable().Count();

            QueryVisualizerWindow window = new QueryVisualizerWindow(query);
            this.MainWindow = window;
            this.MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
