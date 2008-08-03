using System;

namespace Bindable.Linq.Collections
{
    /// <summary>
    /// Represents an element within a source collection.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class SourceElementDescriptor<TElement>
    {
        private readonly int _sourceIndex;
        private readonly TElement _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceElementDescriptor&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="sourceIndex">Index of the source.</param>
        /// <param name="item">The item.</param>
        public SourceElementDescriptor(int sourceIndex, TElement item)
        {
            _sourceIndex = sourceIndex;
            _item = item;
        }

        /// <summary>
        /// Gets the index of the item in the source collection.
        /// </summary>
        public int SourceIndex
        {
            get { return _sourceIndex; }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        public TElement Item
        {
            get { return _item; }
        }
    }
}
