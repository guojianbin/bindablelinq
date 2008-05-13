using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Bindable.Linq;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// An Iterator that reads the the enumerator of the source collection on a background thread, 
    /// using the advantages of data binding and <see cref="T:INotifyCollectionChanged"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of source item.</typeparam>
    internal sealed class AsynchronousIterator<TElement> : 
        Iterator<TElement, TElement> 
        where TElement : class
    {
        private StateScope _loadingState;
        private IteratorThread _iteratorThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AsynchronousIterator`1"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public AsynchronousIterator(IBindableCollection<TElement> source) : base(source)
        {

        }

        /// <summary>
        /// When implemented in a derived class, processes all items in the <see cref="P:SourceCollection"/>.
        /// </summary>
        protected override void LoadSourceCollection()
        {
            using (this.IteratorLock.Enter(this))
            {
                if (_iteratorThread != null)
                {
                    _iteratorThread.Cancel = true;
                }
                _iteratorThread = new IteratorThread();
            }
            _iteratorThread.SourceCollection = this.SourceCollection;
            _iteratorThread.YieldCallback =
                delegate(TElement element)
                {
                    this.ReactToAddRange(0, new TElement[] { element });
                };

            Thread t = new Thread(_iteratorThread.Iterate);
            t.Start();
        }

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="addedItems">The added items.</param>
        protected override void ReactToAddRange(int sourceStartingIndex, IEnumerable<TElement> addedItems)
        {
            List<TElement> elementsToAdd = addedItems.EnumerateSafely();
            using (var recorder = this.ResultCollection.Record())
            {
                using (this.IteratorLock.Enter(this))
                {
                    foreach (TElement element in elementsToAdd)
                    {
                        if (!this.ResultCollection.Contains(element))
                        {
                            this.ResultCollection.Add(element, recorder);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="movedItems">The moved items.</param>
        protected override void ReactToMoveRange(int sourceStartingIndex, IEnumerable<TElement> movedItems)
        {
        }

        /// <summary>
        /// When overridden in a derived class, processes a Remove event over a range of items.
        /// </summary>
        /// <param name="removedItems">The removed items.</param>
        protected override void ReactToRemoveRange(IEnumerable<TElement> removedItems)
        {
            List<TElement> elementsToRemove = removedItems.EnumerateSafely();
            using (var recorder = this.ResultCollection.Record())
            {
                using (this.IteratorLock.Enter(this))
                {
                    this.ResultCollection.RemoveRange(elementsToRemove, recorder);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, processes a Replace event over a range of items.
        /// </summary>
        /// <param name="oldItems">The old items.</param>
        /// <param name="newItems">The new items.</param>
        protected override void ReactToReplaceRange(IEnumerable<TElement> oldItems, IEnumerable<TElement> newItems)
        {
            List<TElement> oldElements = oldItems.EnumerateSafely();
            List<TElement> newElements = newItems.EnumerateSafely();
            using (var recorder = this.ResultCollection.Record())
            {
                using (this.IteratorLock.Enter(this))
                {
                    this.ResultCollection.ReplaceRange(oldElements, newElements, new List<int>(), recorder);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, processes a PropertyChanged event on a source item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected override void ReactToItemPropertyChanged(TElement item, string propertyName)
        {
            // Nothing to do here
        }

        private class IteratorThread
        {
            private bool _cancel;
            private IBindableCollection<TElement> _sourceCollection;
            private Action<TElement> _yieldCallback;

            public IteratorThread()
            {

            }

            public bool Cancel
            {
                get { return _cancel; }
                set { _cancel = value; }
            }

            public IBindableCollection<TElement> SourceCollection
            {
                get { return _sourceCollection; }
                set { _sourceCollection = value; }
            }

            public Action<TElement> YieldCallback
            {
                get { return _yieldCallback; }
                set { _yieldCallback = value; }
            }

            public void Iterate(object state)
            {
                IEnumerator<TElement> enumerator = this.SourceCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TElement current = enumerator.Current;
                    if (this.Cancel)
                    {
                        break;
                    }
                    if (current != null)
                    {
                        if (this.YieldCallback != null) 
                        {
                            this.YieldCallback(current);
                        }
                    }
                }
            }
        }
    }
}