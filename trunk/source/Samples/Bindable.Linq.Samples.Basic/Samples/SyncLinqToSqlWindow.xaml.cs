using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BindingOriented.SyncLinq;
using BindingOriented.SyncLinq.SampleApplication.AdventureWorksSample;

namespace BindingOriented.SyncLinq.SampleApplication.Samples
{
    public partial class SyncLinqToSqlWindow : Window
    {
        public SyncLinqToSqlWindow()
        {
            InitializeComponent();

            AdventureWorksDataContext context = new AdventureWorksDataContext();

            this.DataContext = context.Contacts
                    .Where(c => c.FirstName.StartsWith("p"))
                    .OrderBy(c => c.FirstName)
                    .Take(100)
                    .Select(c => new
                    {
                        Name = c.FirstName + " " + c.LastName,
                        Company = c.EmailAddress,
                        Original = c
                    })
                    .AsBindableAsynchronous();
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((ISyncLinqCollection)this.DataContext).Refresh();
        }
    }
}
