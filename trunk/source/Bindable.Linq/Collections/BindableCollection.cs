using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Bindable.Linq;
using Bindable.Linq.Collections;
using Bindable.Linq.Eventing;
using Bindable.Linq.Helpers;
using Bindable.Linq.Iterators;

namespace Bindable.Linq.Collections
{
    /// <summary>
    /// This class is used as the primary implementation of a bindable collection. Most of the Iterators
    /// in Bindable LINQ use this class eventually to store their bindable results, as it abstracts a lot of the 
    /// logic around adding, replacing, moving and removing collections of items and raising the correct 
    /// events. It is similar to the <see cref="T:ObservableCollection`1"/> in most ways, but provides 
    /// additional functionality.
    /// </summary>
    /// <typeparam name="TElement">The type of item held within the collection.</typeparam>
    public class BindableCollection<TElement> :
        IBindableCollection<TElement>,
        IBindableCollectionInterceptor<TElement>,
        IList<TElement>,
        IEnumerable<TElement>,
        INotifyCollectionChanged,
        INotifyPropertyChanged,
        IList,
        IBindableCollection,
        IDisposable
    {
        private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs("Count");
        private readonly LockScope _bindableCollectionLock = new LockScope();
        private readonly List<TElement> _innerList;
        private readonly IEqualityComparer<TElement> _comparer = ElementComparerFactory.Create<TElement>();
        private readonly SnapshotManager<TElement> _snapshotManager;
        private readonly CollectionChangedPublisher<TElement> _eventPublisher;
        private readonly List<Action<TElement>> _preYieldSteps;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BindableCollection`1"/> class.
        /// </summary>
        public BindableCollection()
        {
            _innerList = new List<TElement>();
            _eventPublisher = new CollectionChangedPublisher<TElement>(this);
            _snapshotManager = new SnapshotManager<TElement>(RebuildSnapshotCallback);
            _preYieldSteps = new List<Action<TElement>>();
        }

        /// <summary>
        /// Gets the item used to lock this collection.
        /// </summary>
        /// <remarks>
        /// TODO: This should not be internal.
        /// </remarks>
        protected internal LockScope BindableCollectionLock
        {
            get { return _bindableCollectionLock; }
        }

        /// <summary>
        /// Gets the <see cref="T:List`1"/> used internally to store the items.
        /// </summary>
        private List<TElement> InnerList
        {
            get { return _innerList; }
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <value>The item at the specified index.</value>
        public TElement this[int index]
        {
            get
            {
                using (this.BindableCollectionLock.Enter(this))
                {
                    return this.InnerList[index];
                }
            }
            set { Replace(this.InnerList[index], value); }
        }

        /// <summary>
        /// Gets the event publisher.
        /// </summary>
        protected ICollectionChangedPublisher<TElement> EventPublisher
        {
            get { return _eventPublisher; }
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        /// <remarks>Warning: No locks should be held when raising this event.</remarks>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _eventPublisher.CollectionChanged += value; }
            remove { _eventPublisher.CollectionChanged -= value; }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <remarks>Warning: No locks should be held when raising this event.</remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether this instance has property changed subscribers.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has property changed subscribers; otherwise, <c>false</c>.
        /// </value>
        internal bool HasPropertyChangedSubscribers
        {
            get { return this.PropertyChanged != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has collection changed subscribers.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has collection changed subscribers; otherwise, <c>false</c>.
        /// </value>
        internal bool HasCollectionChangedSubscribers
        {
            get { return _eventPublisher.HasCollectionChangedSubscribers; }
        }

        #region Add

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(TElement item)
        {
            this.AddRange(new TElement[] { item });
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <param name="recorder">The recorder.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(TElement item, ICollectionChangedRecorder<TElement> recorder)
        {
            this.AddRange(new TElement[] { item }, recorder);
        }

        /// <summary>
        /// Adds a range of items to the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="range">The items to add.</param>
        public void AddRange(params TElement[] range)
        {
            this.AddRange((IEnumerable<TElement>)range);
        }

        /// <summary>
        /// Adds a range of items to the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="range">The items to add.</param>
        public void AddRange(IEnumerable<TElement> range)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                this.AddRange(range, recorder);
            }
        }

        /// <summary>
        /// Adds a range of items to the collection (whilst holding a lock) without raising any collection changed events.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="recorder">The recorder.</param>
        public void AddRange(IEnumerable<TElement> range, ICollectionChangedRecorder<TElement> recorder)
        {
            List<TElement> itemsToAdd = range.EnumerateSafely();

            using (this.BindableCollectionLock.Enter(this))
            {
                foreach (TElement item in itemsToAdd)
                {
                    int index = ((IList)this.InnerList).Add(item);
                    _snapshotManager.Invalidate();
                    recorder.RecordAdd(item, index);
                }
            }
        }

        /// <summary>
        /// Adds or inserts a range of items at a given index (which may be negative, in which case 
        /// the items will be added (appended to the end).
        /// </summary>
        /// <param name="index">The index to add the items at.</param>
        /// <param name="items">The items to add.</param>
        public void AddOrInsertRange(int index, IEnumerable<TElement> items)
        {
            if (index == -1) this.AddRange(items);
            else this.InsertRange(index, items);
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts an item to the <see cref="T:BindableCollection`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:BindableCollection`1"/>.</param>
        public void Insert(int index, TElement item)
        {
            this.InsertRange(index, new TElement[] { item });
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:BindableCollection`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:BindableCollection`1"/>.</param>
        /// <param name="recorder">The recorder.</param>
        public void Insert(int index, TElement item, ICollectionChangedRecorder<TElement> recorder)
        {
            this.InsertRange(index, new TElement[] { item }, recorder);
        }

        /// <summary>
        /// Inserts a range of items into the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="index">The index to start inserting at.</param>
        /// <param name="range">The items to insert into the <see cref="T:BindableCollection`1"/>.</param>
        public void InsertRange(int index, IEnumerable<TElement> range)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                this.InsertRange(index, range, recorder);
            }
        }

        /// <summary>
        /// Inserts a range of items into the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="index">The index to start inserting at.</param>
        /// <param name="range">The items to insert into the <see cref="T:BindableCollection`1"/>.</param>
        /// <param name="recorder">The recorder.</param>
        public void InsertRange(int index, IEnumerable<TElement> range, ICollectionChangedRecorder<TElement> recorder)
        {
            if (index < 0)
            {
                this.AddRange(range, recorder);
                return;
            }

            List<TElement> itemsToInsert = range.EnumerateSafely();

            using (this.BindableCollectionLock.Enter(this))
            {
                if (index > this.InnerList.Count)
                {
                    index = this.InnerList.Count;
                }

                for (int ixCurrentItem = 0; ixCurrentItem < itemsToInsert.Count; ixCurrentItem++)
                {
                    int insertionIndex = index + ixCurrentItem;

                    this.InnerList.Insert(insertionIndex, itemsToInsert[ixCurrentItem]);
                    _snapshotManager.Invalidate();

                    recorder.RecordAdd(itemsToInsert[ixCurrentItem], insertionIndex);
                }
            }
        }

        /// <summary>
        /// Inserts a range of items so that they appear in order, using a given comparer.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="comparer">The comparer.</param>
        public void InsertRangeOrder(IEnumerable<TElement> range, Comparison<TElement> comparer)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                this.InsertRangeOrder(range, comparer, recorder);
            }
        }

        /// <summary>
        /// Inserts a range of items so that they appear in order, using a given comparer.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="recorder">The recorder.</param>
        public void InsertRangeOrder(IEnumerable<TElement> range, Comparison<TElement> comparer, ICollectionChangedRecorder<TElement> recorder)
        {
            List<TElement> itemsToInsert = range.EnumerateSafely();

            using (this.BindableCollectionLock.Enter(this))
            {
                foreach (TElement element in itemsToInsert)
                {
                    bool inserted = false;
                    for (int i = 0; i < this.InnerList.Count; i++)
                    {
                        int result = comparer(element, this.InnerList[i]);
                        if (result <= 0)
                        {
                            this.Insert(i, element, recorder);
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        this.Add(element, recorder);
                    }
                }
            }
        }

        #endregion

        #region Move

        /// <summary>
        /// Moves an item to a new location within the collection.
        /// </summary>
        /// <param name="newIndex">The new index.</param>
        /// <param name="item">The item to move.</param>
        public void Move(int newIndex, TElement item)
        {
            this.MoveRange(newIndex, new TElement[] { item });
        }

        /// <summary>
        /// Moves an item to a new location within the collection.
        /// </summary>
        /// <param name="newIndex">The new index.</param>
        /// <param name="item">The item to move.</param>
        /// <param name="recorder">The recorder.</param>
        public void Move(int newIndex, TElement item, ICollectionChangedRecorder<TElement> recorder)
        {
            this.MoveRange(newIndex, new TElement[] { item }, recorder);
        }

        /// <summary>
        /// Moves a collection of items from their old location (whether the items are contiguous or not) 
        /// to a new starting location (where the items will be contiguous).
        /// </summary>
        /// <param name="range">The items to move.</param>
        /// <param name="newIndex">The new index to move the items to.</param>
        public void MoveRange(int newIndex, IEnumerable<TElement> range)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                this.MoveRange(newIndex, range, recorder);
            }
        }

        /// <summary>
        /// Moves a collection of items from their old location (whether the items are contiguous or not)
        /// to a new starting location (where the items will be contiguous).
        /// </summary>
        /// <param name="newIndex">The new index to move the items to.</param>
        /// <param name="range">The items to move.</param>
        /// <param name="recorder">The recorder.</param>
        /// <remarks>
        /// Here is an example of how this logic works:
        /// Index     Start        Step 1       Step 2
        /// --------------------------------------------
        /// 0:        Paul         Paul         Paul
        /// 1:        Chuck        Larry        Larry
        /// 2:        Larry        Timone       Timone
        /// 3:        Timone       Pumba        Pumba
        /// 4:        Pumba        Patrick      Chuck
        /// 5:        Patrick                   Patrick
        /// Operation: Move "Chuck" from ix=1 to ix=4
        /// 1) Remove "Chuck" - removedIndex = 1
        /// 2) Insert "Chuck" - newIndex = 4
        /// </remarks>
        public void MoveRange(int newIndex, IEnumerable<TElement> range, ICollectionChangedRecorder<TElement> recorder)
        {
            List<TElement> itemsToMove = range.EnumerateSafely();

            using (this.BindableCollectionLock.Enter(this))
            {
                for (int ixCurrentItem = 0; ixCurrentItem < itemsToMove.Count; ixCurrentItem++)
                {
                    TElement element = itemsToMove[ixCurrentItem];
                    int originalIndex = this.IndexOf(element);
                    int desiredIndex = newIndex + ixCurrentItem;

                    // Remove it temporarily
                    bool removed = false;
                    if (originalIndex >= 0)
                    {
                        this.InnerList.Remove(element);
                        removed = true;
                    }

                    // Insert it into the correct spot
                    if (desiredIndex >= this.InnerList.Count)
                    {
                        desiredIndex = ((IList)this.InnerList).Add(element);
                    }
                    else
                    {
                        this.InnerList.Insert(desiredIndex, element);
                    }
                    _snapshotManager.Invalidate();

                    // Record the appropriate event
                    if (removed)
                    {
                        recorder.RecordMove(element, originalIndex, desiredIndex);
                    }
                    else
                    {
                        recorder.RecordAdd(element, desiredIndex);
                    }
                }
            }
        }

        /// <summary>
        /// Moves an item to the correct place if it is no longer in the right place.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="comparer">The comparer.</param>
        public void MoveItemOrdered(TElement item, Comparison<TElement> comparer)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                this.MoveOrdered(item, comparer, recorder);
            }
        }

        /// <summary>
        /// Moves an item to the correct place if it is no longer in the right place.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="recorder">The recorder.</param>
        public void MoveOrdered(TElement element, Comparison<TElement> comparer, ICollectionChangedRecorder<TElement> recorder)
        {
            using (this.BindableCollectionLock.Enter(this))
            {
                int originalIndex = this.IndexOf(element);
                if (originalIndex >= 0)
                {
                    for (int i = 0; i < this.InnerList.Count; i++)
                    {
                        int result = comparer(element, this.InnerList[i]);
                        if (result <= 0)
                        {
                            bool itemAlreadyInOrder = true;
                            for (int j = i; j < originalIndex && j < this.InnerList.Count; j++)
                            {
                                if (comparer(element, this.InnerList[j]) > 0)
                                {
                                    itemAlreadyInOrder = false;
                                    break;
                                }
                            }

                            if (!itemAlreadyInOrder)
                            {
                                if (i != originalIndex)
                                {
                                    this.Move(i, element, recorder);
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region Replace

        /// <summary>
        /// Replaces a given item with another item.
        /// </summary>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        public void Replace(TElement oldItem, TElement newItem)
        {
            this.ReplaceRange(new TElement[] { oldItem }, new TElement[] { newItem });
        }

        /// <summary>
        /// Replaces a given item with another item.
        /// </summary>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        /// <param name="recorder">The recorder.</param>
        public void Replace(TElement oldItem, TElement newItem, ICollectionChangedRecorder<TElement> recorder)
        {
            this.ReplaceRange(new TElement[] { oldItem }, new TElement[] { newItem });
        }

        /// <summary>
        /// Replaces a list of old items with a list of new items.
        /// </summary>
        /// <param name="oldItemsRange">The old items range.</param>
        /// <param name="newItemsRange">The new items range.</param>
        public void ReplaceRange(IEnumerable<TElement> oldItemsRange, IEnumerable<TElement> newItemsRange)
        {
            this.ReplaceRange(oldItemsRange, newItemsRange, new List<int>());
        }

        /// <summary>
        /// Replaces a list of old items with a list of new items.
        /// </summary>
        /// <param name="oldItemsRange">The old items range.</param>
        /// <param name="newItemsRange">The new items range.</param>
        /// <param name="newItemsToSkip">The new items to skip.</param>
        /// <remarks>
        /// TODO: newItemsToSkip is a HACK. Should find a better approach.
        /// </remarks>
        public void ReplaceRange(IEnumerable<TElement> oldItemsRange, IEnumerable<TElement> newItemsRange, List<int> newItemsToSkip)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                this.ReplaceRange(oldItemsRange, newItemsRange, newItemsToSkip, recorder);
            }
        }

        /// <summary>
        /// Replaces a list of old items with a list of new items.
        /// </summary>
        /// <param name="oldItemsRange">The old items range.</param>
        /// <param name="newItemsRange">The new items range.</param>
        /// <param name="newItemsToSkip">The new items to skip.</param>
        /// <param name="recorder">The recorder.</param>
        /// <remarks>
        /// TODO: newItemsToSkip is a HACK. Should find a better approach.
        /// </remarks>
        public void ReplaceRange(IEnumerable<TElement> oldItemsRange, IEnumerable<TElement> newItemsRange, List<int> newItemsToSkip, ICollectionChangedRecorder<TElement> recorder)
        {
            List<TElement> oldItems = oldItemsRange.EnumerateSafely();
            List<TElement> newItems = newItemsRange.EnumerateSafely();

            if (oldItems.Count == 0 && newItems.Count == 0)
            {
                return;
            }

            // Now begin replacing the items. It is safe to acquire a lock at this point as we won't be 
            // touching the source collection nor raising any events (yet).
            using (this.BindableCollectionLock.Enter(this))
            {
                for (int relativeIndex = 0; relativeIndex < oldItems.Count || relativeIndex < newItems.Count; relativeIndex++)
                {
                    object oldItem = (relativeIndex < oldItems.Count) ? (object)oldItems[relativeIndex] : null;
                    object newItem = (relativeIndex < newItems.Count && !newItemsToSkip.Contains(relativeIndex)) ? (object)newItems[relativeIndex] : null;
                    TElement oldElement = (oldItem != null) ? (TElement)oldItem : default(TElement);
                    TElement newElement = (newItem != null) ? (TElement)newItem : default(TElement);

                    if (oldItem != null && newItem != null)
                    {
                        int oldItemIndex = this.IndexOf(oldElement);
                        if (oldItemIndex >= 0)
                        {
                            this.InnerList[oldItemIndex] = newElement;
                            recorder.RecordReplace(oldElement, newElement, oldItemIndex);
                            _snapshotManager.Invalidate();
                        }
                        else
                        {
                            this.Add(newElement, recorder);
                        }
                    }
                    else if (newItem == null && oldItem == null)
                    {
                    }
                    else if (newItem == null)
                    {
                        this.Remove(oldElement, recorder);
                    }
                    else if (oldItem == null)
                    {
                        this.Add(newElement, recorder);
                    }
                }
            }

        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// true if <paramref name="element"/> was successfully removed from the <see cref="T:BindableCollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:BindableCollection`1"/>.
        /// </returns>
        public bool Remove(TElement element)
        {
            return this.RemoveRange(new TElement[] { element });
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="recorder">The recorder.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:BindableCollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:BindableCollection`1"/>.
        /// </returns>
        public bool Remove(TElement element, ICollectionChangedRecorder<TElement> recorder)
        {
            return this.RemoveRange(new TElement[] {element}, recorder);
        }

        /// <summary>
        /// Removes the item at the specified index in the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                TElement item = default(TElement);
                using (this.BindableCollectionLock.Enter(this))
                {
                    item = this.InnerList[index];
                    this.Remove(item, recorder);
                }
            }
        }
        
        /// <summary>
        /// Removes a range of items from the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="range">The items to remove.</param>
        public bool RemoveRange(IEnumerable<TElement> range)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                return this.RemoveRange(range, recorder);
            }
        }

        /// <summary>
        /// Removes a range of items from the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="range">The items to remove.</param>
        /// <param name="recorder">The recorder.</param>
        /// <returns></returns>
        public bool RemoveRange(IEnumerable<TElement> range, ICollectionChangedRecorder<TElement> recorder)
        {
            bool result = false;
            List<TElement> itemsToRemove = range.EnumerateSafely();

            using (this.BindableCollectionLock.Enter(this))
            {
                foreach (TElement element in itemsToRemove)
                {
                    int index = this.IndexOf(element);
                    if (index >= 0)
                    {
                        this.InnerList.RemoveAt(index);
                        _snapshotManager.Invalidate();
                        recorder.RecordRemove(element, index);
                        result = true;
                    }
                }
            }
            return result;
        }

        #endregion

        #region Clear

        /// <summary>
        /// Removes all items from the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        public void Clear()
        {
            using (var recorder = this.EventPublisher.Record())
            {
                this.Clear(recorder);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="recorder">The recorder.</param>
        public void Clear(ICollectionChangedRecorder<TElement> recorder)
        {
            using (this.BindableCollectionLock.Enter(this))
            {
                this.InnerList.Clear();
                _snapshotManager.Invalidate();
                recorder.RecordReset();
            }
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="T:BindableCollection`1"/>. The 
        /// enumerator returned is a special kind of enumerator that allows the collection to be 
        /// modified even while it is being enumerated.
        /// </summary>
        /// <returns>
        /// An <see cref="T:IEnumerator`1"/> that can be used to iterate through the <see cref="T:BindableCollection`1"/> in a thread-safe way.
        /// </returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            using (this.BindableCollectionLock.Enter(this))
            {
                return _snapshotManager.CreateEnumerator();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the <see cref="T:BindableCollection`1"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Rebuilds the snapshot.
        /// </summary>
        private List<TElement> RebuildSnapshotCallback()
        {
            using (this.BindableCollectionLock.Enter(this))
            {
                List<TElement> snapshot = new List<TElement>(this.InnerList.Count);
                snapshot.AddRange(this.InnerList);
                return snapshot;
            }
        }

        #endregion

        #region IList Members

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public int Add(object value)
        {
            using (var recorder = this.EventPublisher.Record())
            {
                using (this.BindableCollectionLock.Enter(this))
                {
                    this.Add((TElement)value, recorder);
                    return this.IndexOf((TElement)value);
                }
            }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        public bool Contains(object value)
        {
            return this.Contains((TElement)value);
        }

        /// <summary>
        /// Determines whether the <see cref="T:BindableCollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:BindableCollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:BindableCollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(TElement item)
        {
            return this.IndexOf(item) >= 0;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
        /// <returns>
        /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(object value)
        {
            return this.IndexOf((TElement)value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:BindableCollection`1"/>.</param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(TElement item)
        {
            using (this.BindableCollectionLock.Enter(this))
            {
                // List<T>.IndexOf(item) underneath uses object.Equals(). We want to use object.ReferenceEquals() so that 
                // overloaded Equals operations do not have an effect. 
                int index = -1;
                for (int i = 0; i < this.InnerList.Count; i++)
                {
                    if (_comparer.Equals(item, this.InnerList[i]))
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to insert into the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        /// <exception cref="T:System.NullReferenceException">
        /// 	<paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        public void Insert(int index, object value)
        {
            this.Insert(index, (TElement)value);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.</returns>
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to remove from the <see cref="T:System.Collections.IList"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public void Remove(object value)
        {
            this.Remove((TElement)value);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:BindableCollection`1"/> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:BindableCollection`1"/> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value></value>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (TElement)value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        public bool IsSynchronized
        {
            get { return true; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
        public object SyncRoot
        {
            get { return new object(); }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:BindableCollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:BindableCollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.-or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
        /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
        public void CopyTo(Array array, int index)
        {
            object[] underlyingArray = (object[])array;
            using (this.BindableCollectionLock.Enter(this))
            {
                for (int arrayIndex = 0, innerIndex = index;
                    innerIndex < this.InnerList.Count && arrayIndex < array.Length;
                    innerIndex++, arrayIndex++)
                {
                    array.SetValue(this.InnerList[innerIndex], arrayIndex);
                }
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:BindableCollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:BindableCollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        public void CopyTo(TElement[] array, int arrayIndex)
        {
            using (this.BindableCollectionLock.Enter(this))
            {
                this.InnerList.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:BindableCollection`1"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:BindableCollection`1"/>.</returns>
        public int Count
        {
            get { return this.InnerList.Count; }
        }

        #endregion

        public void AddPreYieldStep(Action<TElement> step)
        {
            _preYieldSteps.Add(step);
        }

        /// <summary>
        /// Records this instance.
        /// </summary>
        public ICollectionChangedRecorder<TElement> Record()
        {
            return this.EventPublisher.Record();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "BindableCollection - Count: " + this.Count);
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}