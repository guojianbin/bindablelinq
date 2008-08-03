using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Bindable.Linq.Interfaces.Internal;
using Bindable.Linq.Interfaces.Internal.Events;

namespace Bindable.Linq.Adapters.Incoming
{
    /// <summary>
    /// Serves as a base class for adapters that turn outside collections into IInternalCollections.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal abstract class InternalCollectionAdapterBase<TElement> : IInternalCollection<TElement>
    {
        private bool _hasEvaluated;
        private readonly bool _throwOnInvalidCast;
        private IEnumerable _sourceCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalCollectionAdapterBase&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="throwOnInvalidCast">if set to <c>true</c> [throw on invalid cast].</param>
        public InternalCollectionAdapterBase(IEnumerable sourceCollection, bool throwOnInvalidCast)
        {
            _sourceCollection = sourceCollection;
            _throwOnInvalidCast = throwOnInvalidCast;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<TElement> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Refreshes the object.
        /// </summary>
        public void Refresh()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether this instance has already evaluated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has evaluated; otherwise, <c>false</c>.
        /// </value>
        public bool HasEvaluated
        {
            get { return _hasEvaluated; }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the collection is being evaluated (GetEnumerator() is called) for the first time, just before it returns
        /// the results, to provide insight into the items being evaluated. This allows consumers to iterate the items in a collection
        /// just before they are returned to the caller, while still enabling delayed execution of queries.
        /// </summary>
        public event EvaluatingEventHandler<TElement> Evaluating;

        /// <summary>
        /// Occurs when one or more consecutive items are added to the collection.
        /// </summary>
        public event AddedEventHandler<TElement> ItemsAdded;

        /// <summary>
        /// Occurs when one or more consecutive items are removed from the collection.
        /// </summary>
        public event RemovedEventHandler<TElement> ItemsRemoved;

        /// <summary>
        /// Occurs when one or more items in the collection have been replaced.
        /// </summary>
        public event ReplacedEventHandler<TElement> ItemsReplaced;

        /// <summary>
        /// Occurs when one or more items in the collection have been moved.
        /// </summary>
        public event MovedEventHandler<TElement> ItemsMoved;

        /// <summary>
        /// Occurs when the collection has changed significantly and consumers should re-read the collection.
        /// </summary>
        public event ResetEventHandler Reset;

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:Evaluating"/> event.
        /// </summary>
        /// <param name="args">The <see cref="Bindable.Linq.Interfaces.Internal.Events.EvaluatingEventArgs&lt;TElement&gt;"/> instance containing the event data.</param>
        private void OnEvaluating(EvaluatingEventArgs<TElement> args)
        {
            var handler = Evaluating;
            if (handler != null) handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemsAdded"/> event.
        /// </summary>
        /// <param name="args">The <see cref="Bindable.Linq.Interfaces.Internal.Events.AddedEventArgs&lt;TElement&gt;"/> instance containing the event data.</param>
        private void OnItemsAdded(AddedEventArgs<TElement> args)
        {
            var handler = ItemsAdded;
            if (handler != null) handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemsRemoved"/> event.
        /// </summary>
        /// <param name="args">The <see cref="Bindable.Linq.Interfaces.Internal.Events.RemovedEventArgs&lt;TElement&gt;"/> instance containing the event data.</param>
        private void OnItemsRemoved(RemovedEventArgs<TElement> args)
        {
            var handler = ItemsRemoved;
            if (handler != null) handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemsReplaced"/> event.
        /// </summary>
        /// <param name="args">The <see cref="Bindable.Linq.Interfaces.Internal.Events.ReplacedEventArgs&lt;TElement&gt;"/> instance containing the event data.</param>
        private void OnItemsReplaced(ReplacedEventArgs<TElement> args)
        {
            var handler = ItemsReplaced;
            if (handler != null) handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:ItemsMoved"/> event.
        /// </summary>
        /// <param name="args">The <see cref="Bindable.Linq.Interfaces.Internal.Events.MovedEventArgs&lt;TElement&gt;"/> instance containing the event data.</param>
        private void OnItemsMoved(MovedEventArgs<TElement> args)
        {
            var handler = ItemsMoved;
            if (handler != null) handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:Reset"/> event.
        /// </summary>
        /// <param name="args">The <see cref="Bindable.Linq.Interfaces.Internal.ResetEventArgs"/> instance containing the event data.</param>
        private void OnReset(ResetEventArgs args)
        {
            var handler = Reset;
            if (handler != null) handler(this, args);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            
        }
    }
}
