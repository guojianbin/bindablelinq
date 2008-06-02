using System;
using System.Collections.Generic;
using System.Globalization;
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
using Bindable.Linq.Samples.MessengerClient.MessengerService;
using Bindable.Linq;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Samples.MessengerClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MessengerWindow : Window
    {
        private IMessengerService _messengerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessengerWindow"/> class.
        /// </summary>
        /// <param name="messengerService">The messenger service.</param>
        public MessengerWindow(IMessengerService messengerService)
        {
            this.InitializeComponent();

            _messengerService = messengerService;
            
            _messengerService.SignIn("paul", "foo");

            _contactsListBox.ItemsSource = (from c in _messengerService.Contacts.AsBindable().Asynchronous()
                                            where c.Name
                                                 .ToLower(CultureInfo.CurrentUICulture)
                                                 .Contains(_filterTextBox.Text.ToLower(CultureInfo.CurrentUICulture))
                                            orderby c.Name
                                            select c);
        }
    }
}
