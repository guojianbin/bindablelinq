using System;
using System.Collections.Generic;

namespace Bindable.Linq.Interfaces.Internal.Events
{
    /// <summary>
    /// Event handler for the <see cref="RemovedEventArgs{TElement}"/>.
    /// </summary>
    internal delegate void RemovedEventHandler<TElement>(object sender, RemovedEventArgs<TElement> args);

    /// <summary>
    /// Event arguments raised when one or more consecutive items are removed from a collection.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal class RemovedEventArgs<TElement> : EventArgs
    {
        private readonly int _removalStartIndex;
        private readonly List<TElement> _consecutiveRemovedItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemovedEventArgs&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="removalStartIndex">Start index of the removal.</param>
        /// <param name="consecutiveRemovedItems">The consecutive removed items.</param>
        public RemovedEventArgs(int removalStartIndex, List<TElement> consecutiveRemovedItems)
        {
            _removalStartIndex = removalStartIndex;
            _consecutiveRemovedItems = consecutiveRemovedItems;
        }

        /// <summary>
        /// Gets the consecutive list of items that have been removed, beginning at the <see cref="RemovalStartIndex"/>.
        /// </summary>
        /// <value>The consecutive removed items.</value>
        public List<TElement> ConsecutiveRemovedItems
        {
            get { return _consecutiveRemovedItems; }
        }

        /// <summary>
        /// Gets the index at which the first removed item in the list of consecutive removed items was located.
        /// </summary>
        /// <value>The start index of the removal.</value>
        public int RemovalStartIndex
        {
            get { return _removalStartIndex; }
        }
    }
}