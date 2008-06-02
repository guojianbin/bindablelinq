namespace Bindable.Linq.Samples.MessengerClient.Domain
{
    using System.ComponentModel;

    /// <summary>
    /// Represents a contact.
    /// </summary>
    public sealed class Contact : INotifyPropertyChanged
    {
        private string _emailAddress;
        private string _name;
        private byte[] _photo;
        private ContactStatus _status;
        private string _tagLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        public Contact() {}

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>The email address.</value>
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmailAddress"));
            }
        }

        /// <summary>
        /// Gets or sets the tag line.
        /// </summary>
        /// <value>The tag line.</value>
        public string TagLine
        {
            get { return _tagLine; }
            set
            {
                _tagLine = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TagLine"));
            }
        }

        /// <summary>
        /// Gets or sets the photo.
        /// </summary>
        /// <value>The photo.</value>
        public byte[] Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Photo"));
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ContactStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}