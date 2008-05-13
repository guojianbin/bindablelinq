using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Tests.TestObjectModel
{
    /// <summary>
    /// Represents a sample object used for testing against.
    /// </summary>
    public class Contact : BindableObject
    {
        private string _name;
        private string _company;

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        public Contact()
        {

        }

        private static readonly PropertyChangedEventArgs ContactIdPropertyChangedEventArgs = new PropertyChangedEventArgs("ContactId");
        private static readonly PropertyChangedEventArgs NamePropertyChangedEventArgs = new PropertyChangedEventArgs("Name");
        private static readonly PropertyChangedEventArgs CompanyPropertyChangedEventArgs = new PropertyChangedEventArgs("Company");

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(NamePropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        public string Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged(CompanyPropertyChangedEventArgs);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Name == ((Contact)obj).Name && this.Company == ((Contact)obj).Company;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return (this.Name + ":::" + this.Company).GetHashCode();
        }
    }
}
