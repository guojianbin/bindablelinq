using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Bindable.Linq;

namespace Bindable.Linq.SampleApplication.Samples
{
    public partial class SyncLinqFilteredWindow : Window
    {
        private ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();

        public SyncLinqFilteredWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _contacts.Add(new Contact() { Name = "Paul", Company = "Readify" });
            _contacts.Add(new Contact() { Name = "Mitch", Company = "Readify" });
            _contacts.Add(new Contact() { Name = "Darren", Company = "Readify" });
            _contacts.Add(new Contact() { Name = "Richard", Company = "Microsoft" });

            this.DataContext = from c in _contacts.AsBindable()
                               where c.Name.IndexOf(_filterTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0
                                  || c.Company.IndexOf(_filterTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0
                               group c by c.Company into g
                               orderby g.Key
                               select new
                               {
                                   Company = g.Key,
                                   Contacts = g.OrderBy(c => c.Name),
                                   NameLengths = g.Sum(c => c.Name.Length)
                               };
        }

        private void DeleteCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            Contact contactToDelete = e.Parameter as Contact;
            if (contactToDelete != null)
            {
                _contacts.Remove(contactToDelete);
            }
        }
    }
}
