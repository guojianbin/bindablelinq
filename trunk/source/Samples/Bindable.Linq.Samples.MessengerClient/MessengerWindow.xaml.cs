namespace Bindable.Linq.Samples.MessengerClient
{
    using System.Globalization;
    using System.Windows;
    using MessengerService;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MessengerWindow : Window
    {
        private readonly IMessengerService _messengerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessengerWindow"/> class.
        /// </summary>
        /// <param name="messengerService">The messenger service.</param>
        public MessengerWindow(IMessengerService messengerService)
        {
            InitializeComponent();

            _messengerService = messengerService;

            _messengerService.SignIn("paul", "foo");

            _contactsListBox.ItemsSource = (from c in _messengerService.Contacts.AsBindable()
                                            where c.Name.ToLower(CultureInfo.CurrentUICulture).Contains(_filterTextBox.Text.ToLower(CultureInfo.CurrentUICulture))
                                            orderby c.Name
                                            select c);
        }
    }
}