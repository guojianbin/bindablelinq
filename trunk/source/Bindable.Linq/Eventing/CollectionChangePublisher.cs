using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Eventing
{
    /// <summary>
    /// An adapter class that raises collection changed events as either CollectionChanged or ListChanged 
    /// events, for compatibility with Windows Forms or Windows Presentation Foundation. Also abstracts 
    /// inconsistencies between WPF and Silverlight 2.0 Beta.
    /// </summary>
    /// <remarks>
    /// For reference, here is a breakdown of the different constructors available to NotifyCollectionChangedEventArgs:
    /// 
    /// Reset:                    public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action);
    /// Add, Remove, Reset:       public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems);
    /// Add, Remove, Reset:       public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem);
    /// Replace:                  public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems);
    /// Add, Remove, Reset:       public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex);
    /// Add, Remove, Reset:       public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index);
    /// Replace:                  public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem);
    /// Replace:                  public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex);
    /// Move:                     public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex);
    /// Move:                     public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex);
    /// Replace:                  public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index);
    ///
    /// And for ListChangedEventArgs:
    /// 
    /// Property changed:         public ListChangedEventArgs(ListChangedType listChangedType, PropertyDescriptor propDesc);
    /// Add,Remove,Replace:       public ListChangedEventArgs(ListChangedType listChangedType, int newIndex);
    /// Property on element:      public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, PropertyDescriptor propDesc);
    /// Move:                     public ListChangedEventArgs(ListChangedType listChangedType, int newIndex, int oldIndex);
    /// </remarks>
    internal sealed class CollectionChangedPublisher<TElement> : ICollectionChangedPublisher<TElement>
    {
        private readonly object _senderToImpersonate;
        private NotifyCollectionChangedEventHandler _collectionChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedPublisher&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="senderToImpersonate">The sender to impersonate.</param>
        public CollectionChangedPublisher(object senderToImpersonate)
        {
            _senderToImpersonate = senderToImpersonate;
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                _collectionChanged += value;
            }
            remove
            {
                _collectionChanged -= value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has collection changed subscribers.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has collection changed subscribers; otherwise, <c>false</c>.
        /// </value>
        internal bool HasCollectionChangedSubscribers
        {
            get { return _collectionChanged != null; }
        }

        /// <summary>
        /// Converts the event to the appropriate event type based on the needs of subscribers, and raises it.
        /// </summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        public void Raise(NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged(e);
        }

        /// <summary>
        /// Creates a new event recorder.
        /// </summary>
        /// <returns></returns>
        public ICollectionChangedRecorder<TElement> Record()
        {
            return new CollectionChangeRecorder<TElement>(this);
        }

        /// <summary>
        /// Raises the <see cref="E:CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = _collectionChanged;
            if (handler != null)
            {
                handler(_senderToImpersonate, e);
            }
        }
    }
}
