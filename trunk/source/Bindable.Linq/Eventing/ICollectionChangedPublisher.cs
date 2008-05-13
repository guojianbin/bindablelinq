using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Bindable.Linq.Eventing
{
    /// <summary>
    /// This interface is implemented by classes which raise either CollectionChanged or ListChanged events, to 
    /// abstract the differences between collection change events in Windows Forms, WPF and Silverlight.
    /// </summary>
    /// <typeparam name="TElement">The type of the element being changed.</typeparam>
    public interface ICollectionChangedPublisher<TElement>
    {
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        void Raise(NotifyCollectionChangedEventArgs e);

        /// <summary>
        /// Creates a new event recorder.
        /// </summary>
        ICollectionChangedRecorder<TElement> Record();
    }
}
