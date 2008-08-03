using System;
using System.Collections.Generic;

namespace Bindable.Linq.Interfaces.Internal.Events
{
    /// <summary>
    /// Event handler for the <see cref="AddedEventArgs{TElement}"/>.
    /// </summary>
    internal delegate void AddedEventHandler<TElement>(object sender, AddedEventArgs<TElement> args);

    /// <summary>
    /// Event arguments raised when one or more consecutive items are added to a collection.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal class AddedEventArgs<TElement> : EventArgs
    {
        private readonly int _additionStartIndex;
        private readonly List<TElement> _consectiveAddedItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddedEventArgs&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="additionStartIndex">Start index of the addition.</param>
        /// <param name="consectiveAddedItems">The consective added items.</param>
        public AddedEventArgs(int additionStartIndex, List<TElement> consectiveAddedItems)
        {
            _additionStartIndex = additionStartIndex;
            _consectiveAddedItems = consectiveAddedItems;
        }

        /// <summary>
        /// Gets the index in the source collection at which the first item was added (whether it was appended to the end or 
        /// inserted within the collection). All added items represented by this event are guaranteed to be consecutive.
        /// </summary>
        /// <value>The start index of the addition.</value>
        public int AdditionStartIndex
        {
            get { return _additionStartIndex; }
        }

        /// <summary>
        /// Gets the list of consective added items. The items raised in this event are guaranteed to be consecutive within the source
        /// collection, to remove any confusion over how added items and the start index are related.
        /// </summary>
        /// <value>The consective added items.</value>
        public List<TElement> ConsectiveAddedItems
        {
            get { return _consectiveAddedItems; }
        }
    }
}