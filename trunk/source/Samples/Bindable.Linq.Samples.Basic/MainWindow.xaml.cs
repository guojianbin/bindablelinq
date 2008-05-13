using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bindable.Linq.SampleApplication.Samples;
using Bindable.Linq.Dependencies;

namespace Bindable.Linq.SampleApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StandardLinqButton_Click(object sender, RoutedEventArgs e)
        {
            new StandardLinqWindow().Show();
        }

        private void SyncLinqObjectsButton_Click(object sender, RoutedEventArgs e)
        {
            new SyncLinqWindow().Show();
        }

        private void SyncLinqToSqlButton_Click(object sender, RoutedEventArgs e)
        {
            //new SyncLinqToSqlWindow().Show();
        }

        private void SyncLinqFilteringButton_Click(object sender, RoutedEventArgs e)
        {
            new SyncLinqFilteredWindow().Show();
        }

        private void SyncLinqGroupingButton_Click(object sender, RoutedEventArgs e)
        {
            new SyncLinqGroupedWindow().Show();
        }
    }
}
