using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Tests.TestObjectModel
{
    /// <summary>
    /// Base class for sample business objects.
    /// </summary>
    public class BindableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether this instance has property changed subscribers.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has property changed subscribers; otherwise, <c>false</c>.
        /// </value>
        public bool HasPropertyChangedSubscribers
        {
            get
            {
                return PropertyChanged != null;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }
    }
}
