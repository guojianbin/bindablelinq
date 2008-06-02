namespace Bindable.Linq.SampleApplication.Samples
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public partial class StandardLinqWindow : Window
    {
        private readonly ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();

        public StandardLinqWindow()
        {
            InitializeComponent();

            for (var i = 0; i < 5; i++)
            {
                _contacts.Add(new Contact() {Name = "Person", Company = "ppppp" + i.ToString() + "Readify"});
            }

            LoadData();
        }

        private void LoadData()
        {
            DataContext = _contacts.Where(c => c.Name.ToLower().Contains("p")).OrderBy(c => c.Name).Select(c => new {Name = c.Name.ToLower(), Company = c.Company, Original = c});
        }

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _contacts.Add(new Contact() {Name = "Pttttt", Company = "pttttt" + _contacts.Count + "Readify", PhoneNumber = "0410 209 290"});
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadData();
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