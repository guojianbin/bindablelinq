using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Samples.MessengerClient.Domain;
using System.Collections.ObjectModel;

namespace Bindable.Linq.Samples.MessengerClient.MessengerService.Simulator
{
    /// <summary>
    /// An implementation of the messenger service that simulates a messenger client.
    /// </summary>
    internal sealed class SimulationMessengerService : IMessengerService
    {
        private readonly ObservableCollection<Contact> _contacts = new ObservableCollection<Contact>();
        private ContactFactory _contactFactory = new ContactFactory();
        private const int _contactsToGenerate = 31;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimulationMessengerService"/> class.
        /// </summary>
        public SimulationMessengerService()
        {
            
        }

        /// <summary>
        /// Signs in to the service.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void SignIn(string username, string password)
        {
            _contacts.Clear();
            _contactFactory = new ContactFactory();
            for (int i = 0; i < _contactsToGenerate; i++)
            {
                _contacts.Add(_contactFactory.CreateContact());
            }
        }

        /// <summary>
        /// Signs out of the service.
        /// </summary>
        public void SignOut()
        {
            _contacts.Clear();
            _contactFactory.Dispose();
        }

        /// <summary>
        /// Gets all of the current user's contacts.
        /// </summary>
        public ObservableCollection<Contact> Contacts
        {
            get { return _contacts; }
        }

        /// <summary>
        /// Occurs when a user requests to be added to the current user's address list.
        /// </summary>
        public event EventHandler<AddContactEventArgs> AddRequestRecieved;


    }
}
