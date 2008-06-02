namespace Bindable.Linq.SampleApplication
{
    using System.Windows;
    using Samples;

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