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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bindable.Linq;

namespace Bindable.Linq.SampleApplication.Samples
{
    public partial class StandardLinqWindow : Window
    {
        private ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();

        public StandardLinqWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 5; i++)
            {
                _contacts.Add(new Contact() { Name = "Person", Company = "ppppp" + i.ToString() + "Readify" });
            }

            LoadData();
        }

        private void LoadData()
        {
            this.DataContext = _contacts
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
            _contacts.Add(new Contact() { 
                Name = "Pttttt", 
                Company = "pttttt" + _contacts.Count + "Readify", 
                PhoneNumber = "0410 209 290" });
        }

        private void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoadData();
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
