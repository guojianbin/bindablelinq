using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies;
using Bindable.Linq;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// The Iterator created when a Where operation is performed.
    /// </summary>
    /// <typeparam name="TElement">The type of source item being filtered.</typeparam>
    internal sealed class WhereIterator<TElement> : 
        Iterator<TElement, TElement>
        where TElement : class
    {
        private readonly Func<TElement, bool> _predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:WhereIterator`1"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="predicate">The predicate.</param>
        public WhereIterator(IBindableCollection<TElement> sourceCollection,
            Func<TElement, bool> predicate) : base(sourceCollection)
        {
            _predicate = predicate;
        }

        /// <summary>
        /// When implemented in a derived class, processes all items in the <see cref="P:SourceCollection"/>.
        /// </summary>
        protected override void LoadSourceCollection()
        {
            this.ReactToAddRange(0, this.SourceCollection);
        }

        /// <summary>
        /// Filters an item from the source collection.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        /// <value>The predicate.</value>
        public bool Filter(TElement element)
        {
            return _predicate(element);
        }

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="addedItems">The added items.</param>
        protected override void ReactToAddRange(int sourceStartingIndex,
            IEnumerable<TElement> addedItems)
        {
            this.ResultCollection.AddOrInsertRange(sourceStartingIndex, addedItems.Where(this.Filter));
        }

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="movedItems">The moved items.</param>
        protected override void ReactToMoveRange(int sourceStartingIndex,
            IEnumerable<TElement> movedItems)
        {
            this.ResultCollection.MoveRange(sourceStartingIndex, movedItems.Where(this.Filter));
        }

        /// <summary>
        /// When overridden in a derived class, processes a Remove event over a range of items.
        /// </summary>
        /// <param name="removedItems">The removed items.</param>
        protected override void ReactToRemoveRange(IEnumerable<TElement> removedItems)
        {
            this.ResultCollection.RemoveRange(removedItems);
        }

        /// <summary>
        /// When overridden in a derived class, processes a Replace event over a range of items.
        /// </summary>
        /// <param name="oldItems">The old items.</param>
        /// <param name="newItems">The new items.</param>
        protected override void ReactToReplaceRange(IEnumerable<TElement> oldItems,
            IEnumerable<TElement> newItems)
        {
            List<int> indexesToSkip = new List<int>();

            int relativeIndex = 0;
            foreach (TElement element in newItems)
            {
                if (!this.Filter(element))
                {
                    indexesToSkip.Add(relativeIndex);
                }
                relativeIndex++;
            }

            this.ResultCollection.ReplaceRange(oldItems, newItems, indexesToSkip);
        }

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ReactToItemPropertyChanged(TElement item, string propertyName)
        {
            if (!this.Filter(item))
            {
                if (this.ResultCollection.Contains(item))
                {
                    this.ResultCollection.Remove(item);
                }
            }
            else
            {
                if (!this.ResultCollection.Contains(item))
                {
                    this.ResultCollection.Add(item);
                }
            }
        }
    }
}