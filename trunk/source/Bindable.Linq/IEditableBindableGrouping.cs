using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq
{
    internal interface IEditableBindableGrouping<TKey, TElement> : IBindableGrouping<TKey, TElement>
    {
        /// <summary>
        /// Adds a range of elements to the collection.
        /// </summary>
        /// <param name="elements">The elements.</param>
        void AddRange(IEnumerable<TElement> elements);

        /// <summary>
        /// Removes a range of elements from the collection.
        /// </summary>
        /// <param name="elements">The elements.</param>
        void RemoveRange(IEnumerable<TElement> elements);

        /// <summary>
        /// Replaces a range of elements in the collection with another range.
        /// </summary>
        /// <param name="oldItems">The old items.</param>
        /// <param name="newItems">The new items.</param>
        void ReplaceRange(IEnumerable<TElement> oldItems,
            IEnumerable<TElement> newItems);
    }
}