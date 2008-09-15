namespace Bindable.Linq.SampleApplication
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Bindable.Linq.Interfaces;

    public partial class SyncLinqWindow : Window
    {
        private readonly ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();

        public SyncLinqWindow()
        {
            InitializeComponent();

            for (var i = 0; i < 5; i++)
            {
                _contacts.Add(new Contact() {Name = "Person", Company = "ppppp" + i.ToString() + "Readify"});
            }

            DataContext = _contacts.AsBindable()
                                   .Where(c => c.Name.ToLower().Contains("p"))
                                   .OrderBy(c => c.Name)
                                   .Select(c => new
                                                    {
                                                        Name = c.Name.ToLower(), 
                                                        Company = c.Company, 
                                                        Original = c
                                                    });
        }

        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _contacts.Add(new Contact() {Name = "Pttttt", Company = "pttttt" + _contacts.Count + "Readify", PhoneNumber = "0410 209 290"});
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((IBindableCollection) DataContext).Refresh();
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