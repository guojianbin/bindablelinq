using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Bindable.Linq.Samples.Silverlight
{
    /// <summary>
    /// Represents a contact.
    /// </summary>
    public class Contact : INotifyPropertyChanged
    {
        private string _name;
        private string _company;
        private string _phoneNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        public Contact()
        {

        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name ?? ""; }
            set
            {
                _name = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        public string PhoneNumber
        {
            get { return _phoneNumber ?? ""; }
            set
            {
                _phoneNumber = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("PhoneNumber"));
            }
        }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company
        {
            get { return _company ?? ""; }
            set
            {
                _company = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Company"));
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
