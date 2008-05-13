using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Samples.MessengerClient.Domain;
using System.Collections.ObjectModel;

namespace Bindable.Linq.Samples.MessengerClient.MessengerService
{
    /// <summary>
    /// This interface is implemented by classes that communicate to a messenger client. For this sample,
    /// the only implementation is a simulation.
    /// </summary>
    public interface IMessengerService
    {
        /// <summary>
        /// Signs in to the service.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        void SignIn(string username, string password);

        /// <summary>
        /// Signs out of the service.
        /// </summary>
        void SignOut();

        /// <summary>
        /// Gets all of the current user's contacts.
        /// </summary>
        ObservableCollection<Contact> Contacts { get; }

        /// <summary>
        /// Occurs when a user requests to be added to the current user's address list.
        /// </summary>
        event EventHandler<AddContactEventArgs> AddRequestRecieved;
    }
}
