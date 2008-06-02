namespace Bindable.Linq.SampleApplication.Samples
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public partial class SyncLinqFilteredWindow : Window
    {
        private readonly ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();

        public SyncLinqFilteredWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _contacts.Add(new Contact() {Name = "Paul", Company = "Readify"});
            _contacts.Add(new Contact() {Name = "Mitch", Company = "Readify"});
            _contacts.Add(new Contact() {Name = "Darren", Company = "Readify"});
            _contacts.Add(new Contact() {Name = "Richard", Company = "Microsoft"});

            DataContext = from c in _contacts.AsBindable()
                          where c.Name.IndexOf(_filterTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0 || c.Company.IndexOf(_filterTextBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0
                          group c by c.Company
                          into g orderby g.Key select new {Company = g.Key, Contacts = g.OrderBy(c => c.Name), NameLengths = g.Sum(c => c.Name.Length)};
        }

        private void DeleteCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            var contactToDelete = e.Parameter as Contact;
            if (contactToDelete != null)
            {
                _contacts.Remove(contactToDelete);
            }
        }
    }
}