using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Eventing
{
    /// <summary>
    /// An interface implemented by classes that record collection changed events and package them up, before they are 
    /// raised.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public interface ICollectionChangedRecorder<TElement> : IDisposable
    {
        /// <summary>
        /// Records the fact that an element was added.
        /// </summary>
        /// <param name="elementAdded">The element added.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentNullException"><paramref name="elementAdded"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        void RecordAdd(TElement elementAdded, int index);

        /// <summary>
        /// Records the fact that an element was moved.
        /// </summary>
        /// <param name="elementMoved">The element moved.</param>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <exception cref="ArgumentNullException"><paramref name="elementMoved"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="newIndex"/> is less than zero.</exception>
        void RecordMove(TElement elementMoved, int oldIndex, int newIndex);

        /// <summary>
        /// Records the fact that an element was removed.
        /// </summary>
        /// <param name="removedElement">The removed element.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentNullException"><paramref name="removedElement"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        void RecordRemove(TElement removedElement, int index);

        /// <summary>
        /// Records the fact that an element was replaced.
        /// </summary>
        /// <param name="originalElement">The original element.</param>
        /// <param name="replacementElement">The replacement element.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="ArgumentNullException"><paramref name="originalElement"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="replacementElement"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        void RecordReplace(TElement originalElement, TElement replacementElement, int index);

        /// <summary>
        /// Records the fact that the collection has changed dramatically and should be refreshed.
        /// </summary>
        void RecordReset();

        /// <summary>
        /// Raises the events. It is imperative that the caller does not hold any locks at this point.
        /// </summary>
        void RaiseAll();
    }
}
