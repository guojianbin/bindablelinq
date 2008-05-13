using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Samples.MessengerClient.Domain;
using System.ComponentModel;

namespace Bindable.Linq.Samples.MessengerClient.MessengerService
{
    /// <summary>
    /// Represents the arguments of an event relating to messenger contacts.
    /// </summary>
    public class AddContactEventArgs : CancelEventArgs
    {
        private Contact _contact;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactEventArgs"/> class.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public AddContactEventArgs(Contact contact)
        {
            _contact = contact;
        }

        /// <summary>
        /// Gets the contact.
        /// </summary>
        public Contact Contact
        {
            get { return _contact; }
        }
    }
}
