using System;
using System.Collections.Generic;

namespace Bindable.Linq.Interfaces.Internal.Events
{
    /// <summary>
    /// Event handler for the <see cref="MovedEventArgs{TElement}"/>.
    /// </summary>
    internal delegate void MovedEventHandler<TElement>(object sender, MovedEventArgs<TElement> args);

    /// <summary>
    /// Event arguments raised when one or more items are moved within a collection. 
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal class MovedEventArgs<TElement> : EventArgs
    {
        private readonly List<Movement> _movements;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovedEventArgs&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="movements">The movements.</param>
        public MovedEventArgs(List<Movement> movements)
        {
            _movements = movements;
        }

        /// <summary>
        /// Gets the list of item movements that have taken place.
        /// </summary>
        /// <value>The movements.</value>
        public List<Movement> Movements
        {
            get { return _movements; }
        }

        /// <summary>
        /// Represents the movement of a single item.
        /// </summary>
        internal class Movement
        {
            private readonly int _oldIndex;
            private readonly int _newIndex;
            private readonly TElement _movedItem;

            /// <summary>
            /// Initializes a new instance of the <see cref="MovedEventArgs&lt;TElement&gt;.Movement"/> class.
            /// </summary>
            /// <param name="oldIndex">The old index.</param>
            /// <param name="newIndex">The new index.</param>
            /// <param name="movedItem">The moved item.</param>
            public Movement(int oldIndex, int newIndex, TElement movedItem)
            {
                _oldIndex = oldIndex;
                _movedItem = movedItem;
                _newIndex = newIndex;
            }

            /// <summary>
            /// Gets item that was moved from the <see cref="OldIndex"/> to the <see cref="NewIndex"/>.
            /// </summary>
            /// <value>The moved item.</value>
            public TElement MovedItem
            {
                get { return _movedItem; }
            }

            /// <summary>
            /// Gets the new index of the <see cref="MovedItem"/>.
            /// </summary>
            /// <value>The new index.</value>
            public int NewIndex
            {
                get { return _newIndex; }
            }

            /// <summary>
            /// Gets the old (original) index of the <see cref="MovedItem"/>.
            /// </summary>
            /// <value>The old index.</value>
            public int OldIndex
            {
                get { return _oldIndex; }
            }
        }
    }
}