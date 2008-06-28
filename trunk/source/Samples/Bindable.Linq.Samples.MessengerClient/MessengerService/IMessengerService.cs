namespace Bindable.Linq.Samples.MessengerClient.MessengerService
{
    using System;
    using System.Collections.ObjectModel;
    using Domain;

    /// <summary>
    /// This interface is implemented by classes that communicate to a messenger client. For this sample,
    /// the only implementation is a simulation.
    /// </summary>
    public interface IMessengerService
    {
        /// <summary>
        /// Occurs when a user requests to be added to the current user's address list.
        /// </summary>
        event EventHandler<AddContactEventArgs> AddRequestReceived;

        /// <summary>
        /// Gets all of the current user's contacts.
        /// </summary>
        ObservableCollection<Contact> Contacts { get; }

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
    }
}