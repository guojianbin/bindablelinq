using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Bindable.Linq.Samples.Silverlight
{
    public partial class Page : UserControl
    {
        private ObservableCollection<Contact> _contacts;

        public Page()
        {
            InitializeComponent();
            _contacts = new ObservableCollection<Contact>();
            _contacts.Add(new Contact() {Name = "Paul Stovell", Company = "Readify", PhoneNumber = "0421 938 793"});
            _contacts.Add(new Contact() {Name = "Omar Besiso", Company = "Readify", PhoneNumber = "0421 938 793"});
            _contacts.Add(new Contact() {Name = "Darren Neimke", Company = "Readify", PhoneNumber = "0421 938 793"});
            _contacts.Add(new Contact() {Name = "Mitch Denny", Company = "Readify", PhoneNumber = "0421 938 793"});
            _contacts.Add(new Contact() {Name = "Richard Banks", Company = "Readify", PhoneNumber = "0421 938 793"});
            _contacts.Add(new Contact() {Name = "Andrew Matthews", Company = "Readify", PhoneNumber = "0421 938 793"});

            _contactsList.ItemsSource = _contacts.AsBindable()
                                        .Where(c => c.Name.ToLower().StartsWith(this.FilterTextBox.Text.ToLower()))
                                        .OrderBy(c => c.Name.ToLower());

            _newItemPanel.DataContext = new Contact();
        }

        public TextBox FilterTextBox
        {
            get { return _filterTextBox; }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Contact contact = (Contact)_newItemPanel.DataContext;
            _contacts.Add(contact);
            _newItemPanel.DataContext = new Contact();
        }
    }
}
