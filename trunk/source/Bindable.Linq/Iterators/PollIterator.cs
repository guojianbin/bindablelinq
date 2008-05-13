using System;
using System.Collections.Generic;
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
    /// An Iterator that reads items from the source collection directly into the results collection, 
    /// and then continues to poll the source collection for changes at a given interval.
    /// </summary>
    /// <typeparam name="TElement">The type of source item.</typeparam>
    internal sealed class PollIterator<TElement> : 
        Iterator<TElement, TElement>
        where TElement : class
    {
        private readonly WeakTimer _timer;
        private readonly Action _reloadCallback;
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="PollIterator&lt;S&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="pollTime">The poll time.</param>
        public PollIterator(IBindableCollection<TElement> sourceCollection, Dispatcher dispatcher, TimeSpan pollTime)
            : base(sourceCollection)
        {
            _dispatcher = dispatcher;
            _reloadCallback = this.Timer_Tick;
            _timer = new WeakTimer(pollTime, _reloadCallback);
        }

        /// <summary>
        /// When implemented in a derived class, processes all items in the <see cref="P:SourceCollection"/>.
        /// </summary>
        protected override void LoadSourceCollection()
        {
            this.ReactToAddRange(0, this.SourceCollection);

            _timer.Start();
        }

        private void Timer_Tick()
        {
            Delegate callback = new Action(this.Reload);
#if !SILVERLIGHT
            _dispatcher.BeginInvoke(DispatcherPriority.Background, callback);
#endif
        }

        private void Reload()
        {
            _timer.Pause();

            // Read all of the items out of the source collections first before entering any locks. 
            // This avoids deadlock situations occurring where the enumerators of the source collections
            // on another thread need a property on this object locked by this thread whilst we wait 
            // for them to return.
            List<TElement> allSourceItems = new List<TElement>();
            using (this.IsLoadingState.Enter())
            {
                foreach (TElement item in this.SourceCollection)
                {
                    allSourceItems.Add(item);
                }
            }

            // Now it is safe to acquire a lock and to decide whether to add/remove the items
            using (var recorder = this.ResultCollection.Record())
            {
                using (this.IteratorLock.Enter(this))
                {
                    foreach (TElement item in allSourceItems)
                    {
                        if (!this.ResultCollection.Contains(item))
                        {
                            this.ResultCollection.Add(item, recorder);
                        }
                    }
                    foreach (TElement item in this.ResultCollection)
                    {
                        if (!allSourceItems.Contains(item))
                        {
                            this.ResultCollection.Remove(item, recorder);
                        }
                    }
                }
            }

            _timer.Continue();
        }

        /// <summary>
        /// When overridden in a derived class, processes an Add event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="addedItems">The added items.</param>
        protected override void ReactToAddRange(int sourceStartingIndex,
            IEnumerable<TElement> addedItems)
        {
            this.ResultCollection.AddOrInsertRange(sourceStartingIndex, addedItems);
        }

        /// <summary>
        /// When overridden in a derived class, processes a Move event over a range of items.
        /// </summary>
        /// <param name="sourceStartingIndex">Index of the source starting.</param>
        /// <param name="movedItems">The moved items.</param>
        protected override void ReactToMoveRange(int sourceStartingIndex,
            IEnumerable<TElement> movedItems)
        {
            this.ResultCollection.MoveRange(sourceStartingIndex, movedItems);
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
            this.ResultCollection.ReplaceRange(oldItems, newItems);
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

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to dispose any expensive components.
        /// </summary>
        protected override void DisposeOverride()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
        }
    }
}