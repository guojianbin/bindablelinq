namespace Bindable.Linq.SampleApplication.Samples
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public partial class SyncLinqGroupedWindow : Window
    {
        public static readonly DependencyProperty NewContactProperty = DependencyProperty.Register("NewContact", typeof (Contact), typeof (SyncLinqGroupedWindow), new UIPropertyMetadata(null));
        private readonly ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();

        public SyncLinqGroupedWindow()
        {
            InitializeComponent();

            _contacts.Add(new Contact() {Name = "Paul Stovell", Company = "Readify"});
            _contacts.Add(new Contact() {Name = "Scott Guthrie", Company = "Microsoft"});
            _contacts.Add(new Contact() {Name = "Darren Neimke", Company = "Readify"});
            _contacts.Add(new Contact() {Name = "Omar Besiso", Company = "Readify"});
            _contacts.Add(new Contact() {Name = "Oren Eini", Company = "We!"});

            DataContext = from c in _contacts
                          group c by c.Company
                          into g orderby g.Key select new {Company = g.Key, Contacts = g.OrderBy(c => c.Name), NameLengths = g.Sum(c => c.Name.Length)};

            NewContact = new Contact();
        }

        public Contact NewContact
        {
            get { return (Contact) GetValue(NewContactProperty); }
            set { SetValue(NewContactProperty, value); }
        }

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _contacts.Add(NewContact);
            NewContact = new Contact();
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((IBindableQuery) DataContext).Refresh();
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var parameter = e.Parameter as Contact;
            if (parameter != null)
            {
                _contacts.Remove(parameter);
            }
        }
    }
}