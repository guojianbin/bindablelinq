using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using System.Linq.Expressions;
using Bindable.Linq.Collections;
using Bindable.Linq;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// An Iterator that reads items from the source collection and groups them by a common key. 
    /// </summary>
    internal sealed class GroupByIterator<TKey, TSource, TElement> :
        Iterator<TSource, IBindableGrouping<TKey, TElement>>
        where TSource : class
    {
        private readonly Expression<Func<TSource, TElement>> _elementSelector;
        private readonly IEqualityComparer<TKey> _keyComparer;
        private readonly Expression<Func<TSource, TKey>> _keySelector;
        private readonly Func<TSource, TKey> _keySelectorCompiled;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupByIterator&lt;TKey, TSource, TElement&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <param name="keyComparer">The key comparer.</param>
        public GroupByIterator(IBindableCollection<TSource> sourceCollection, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> keyComparer) : base(sourceCollection)
        {
            _keySelector = keySelector;
            _keySelectorCompiled = keySelector.Compile();
            _elementSelector = elementSelector;
            _keyComparer = keyComparer;
        }

        /// <summary>
        /// When implemented in a derived class, processes all items in a given source collection.
        /// </summary>
        /// <remarks>Warning: No locks should be held when invoking this method.</remarks>
        protected override void LoadSourceCollection()
        {
            this.ReactToAddRange(0, this.SourceCollection);
        }

        /// <summary>
        /// Extracts a key from a given source item.
        /// </summary>
        /// <param name="sourceItem">The source item.</param>
        /// <returns></returns>
        public TKey KeySelector(TSource sourceItem)
        {
            return _keySelectorCompiled(sourceItem);
        }

        /// <summary>
        /// Compares two keys.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        private bool CompareKeys(TKey lhs,
            TKey rhs)
        {
            return _keyComparer.Equals(lhs, rhs);
        }

        private bool FindGroup(TKey key,
            IEnumerable<IBindableGrouping<TKey, TElement>> groups)
        {
            foreach (IBindableGrouping<TKey, TSource> existingGroup in groups)
            {
                if (CompareKeys(existingGroup.Key, key))
                {
                    return true;
                }
            }
            return false;
        }

        private void EnsureGroupsExists(IEnumerable<TSource> range)
        {
            List<TSource> itemsToCreateGroupsFor = new List<TSource>();
            foreach (TSource element in range)
            {
                itemsToCreateGroupsFor.Add(element);
            }

            List<IBindableGrouping<TKey, TElement>> groupsToAdd = new List<IBindableGrouping<TKey, TElement>>();
            foreach (TSource element in itemsToCreateGroupsFor)
            {
                TKey key = _keySelectorCompiled(element);
                bool groupExists = false;
                using (this.ResultCollection.BindableCollectionLock.Enter(this))
                {
                    groupExists = FindGroup(key, groupsToAdd) || FindGroup(key, this.ResultCollection);
                }
                if (!groupExists)
                {
                    IBindableGrouping<TKey, TElement> newGroup = new BindableGrouping<TKey, TElement>(key, 
                        this.SourceCollection
                        .Where(e => CompareKeys(_keySelectorCompiled(e), key))
                        .WithDependencyExpression(_keySelector.Body, _keySelector.Parameters[0])
                        .Select(_elementSelector));
                    newGroup.CollectionChanged += new NotifyCollectionChangedEventHandler(Group_CollectionChanged);
                    groupsToAdd.Add(newGroup);
                }
            }
            this.ResultCollection.AddRange(groupsToAdd);
        }

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="addedItems">The added items.</param>
        protected override void ReactToAddRange(int sourceStartingIndex,
            IEnumerable<TSource> addedItems)
        {
            this.EnsureGroupsExists(addedItems);
        }

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="movedItems">The moved items.</param>
        protected override void ReactToMoveRange(int sourceStartingIndex, IEnumerable<TSource> movedItems)
        {
            // Nothing to do here
        }

        /// <summary>
        /// When overridden in a derived class, processes a Remove event over a range of items.
        /// </summary>
        /// <param name="removedItems">The removed items.</param>
        protected override void ReactToRemoveRange(IEnumerable<TSource> removedItems)
        {
            // Nothing to do here
        }

        /// <summary>
        /// When overridden in a derived class, processes a Replace event over a range of items.
        /// </summary>
        /// <param name="oldItems">The old items.</param>
        /// <param name="newItems">The new items.</param>
        protected override void ReactToReplaceRange(IEnumerable<TSource> oldItems,
            IEnumerable<TSource> newItems)
        {
            this.EnsureGroupsExists(newItems);
        }

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ReactToItemPropertyChanged(TSource item, string propertyName)
        {
            if (this.SourceCollection.Contains(item).Current)
            {
                this.EnsureGroupsExists(new TSource[] { item });
            }
        }

        private void Group_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                IBindableGrouping<TKey, TElement> group = sender as IBindableGrouping<TKey, TElement>;
                if (group.Count == 0)
                {
                    this.ResultCollection.Remove(group);
                }
            }
        }
    }
}