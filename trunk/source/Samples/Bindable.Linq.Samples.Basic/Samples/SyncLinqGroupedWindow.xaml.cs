using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Bindable.Linq;

namespace Bindable.Linq.SampleApplication.Samples
{
    public partial class SyncLinqGroupedWindow : Window
    {
        private ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();
        
        public SyncLinqGroupedWindow()
        {
            InitializeComponent();

            _contacts.Add(new Contact() { Name = "Paul Stovell", Company = "Readify" });
            _contacts.Add(new Contact() { Name = "Scott Guthrie", Company = "Microsoft" });
            _contacts.Add(new Contact() { Name = "Darren Neimke", Company = "Readify" });
            _contacts.Add(new Contact() { Name = "Omar Besiso", Company = "Readify" });
            _contacts.Add(new Contact() { Name = "Oren Eini", Company = "We!" });

            this.DataContext = from c in _contacts
                               group c by c.Company into g
                               orderby g.Key
                               select new
                               {
                                   Company = g.Key,
                                   Contacts = g.OrderBy(c => c.Name),
                                   NameLengths = g.Sum(c => c.Name.Length)
                               };

            this.NewContact = new Contact();
        }

        public static readonly DependencyProperty NewContactProperty = DependencyProperty.Register("NewContact", typeof(Contact), typeof(SyncLinqGroupedWindow), new UIPropertyMetadata(null));

        public Contact NewContact
        {
            get { return (Contact)GetValue(NewContactProperty); }
            set { SetValue(NewContactProperty, value); }
        }

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _contacts.Add(this.NewContact);
            this.NewContact = new Contact();
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((IBindableQuery)this.DataContext).Refresh();
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Contact parameter = e.Parameter as Contact;
            if (parameter != null)
            {
                _contacts.Remove(parameter);
            }
        }
    }
}
