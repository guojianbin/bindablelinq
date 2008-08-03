using System;
using System.Collections.Generic;

namespace Bindable.Linq.Interfaces.Internal.Events
{
    /// <summary>
    /// Event handler for the <see cref="ReplacedEventArgs{TElement}"/>.
    /// </summary>
    internal delegate void ReplacedEventHandler<TElement>(object sender, ReplacedEventArgs<TElement> args);

    /// <summary>
    /// Event arguments raised when a set of elements within a collection are replaced.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal class ReplacedEventArgs<TElement> : EventArgs
    {
        private readonly List<Replacement> _replacements;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplacedEventArgs&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="replacements">The replacements.</param>
        public ReplacedEventArgs(List<Replacement> replacements)
        {
            _replacements = replacements;
        }

        /// <summary>
        /// Gets the list of replacements.
        /// </summary>
        /// <value>The replacements.</value>
        public List<Replacement> Replacements
        {
            get { return _replacements; }
        }

        /// <summary>
        /// Represents a single replacement operation.
        /// </summary>
        internal class Replacement
        {
            private readonly TElement _oldItem;
            private readonly TElement _newItem;
            private readonly int _index;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReplacedEventArgs&lt;TElement&gt;.Replacement"/> class.
            /// </summary>
            /// <param name="oldItem">The old item.</param>
            /// <param name="newItem">The new item.</param>
            /// <param name="index">The index.</param>
            public Replacement(TElement oldItem, TElement newItem, int index)
            {
                _oldItem = oldItem;
                _index = index;
                _newItem = newItem;
            }

            /// <summary>
            /// Gets the old (original) item.
            /// </summary>
            /// <value>The old item.</value>
            public TElement OldItem

            {
                get { return _oldItem; }
            }

            /// <summary>
            /// Gets the new (replacement) item.
            /// </summary>
            /// <value>The new item.</value>
            public TElement NewItem
            {
                get { return _newItem; }
            }

            /// <summary>
            /// Gets the index of the item.
            /// </summary>
            /// <value>The index.</value>
            public int Index
            {
                get { return _index; }
            }
        }
    }
}