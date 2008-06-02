namespace Bindable.Linq.Tests.TestHelpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Dependencies;

    /// <summary>
    /// This class acts as a helper for testing Bindable LINQ queries against their LINQ counterparts.
    /// </summary>
    /// <typeparam name="TElement">The type of the element returned by the queries.</typeparam>
    internal class IteratorTestState<TInput, TResult> : IDisposable
    {
        private Stack<IDependencyDefinition> _dependencies = new Stack<IDependencyDefinition>();
        private CollectionEventCatcher _eventCatcher;
        private IBindableCollection<TResult> _syncLinqQuery;
        private WeakReference _syncLinqQueryReference;

        /// <summary>
        /// Gets or sets whether or not order is important when comparing the Bindable LINQ query to the LINQ 
        /// query.
        /// </summary>
        public CompatibilityExpectation CompatibilityExpectation { get; set; }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        public IEnumerable<TInput> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the Bindable LINQ query.
        /// </summary>
        public IBindableCollection<TResult> SyncLinqQuery
        {
            get { return _syncLinqQuery; }
            set
            {
                _syncLinqQuery = value;
                _eventCatcher = new CollectionEventCatcher(_syncLinqQuery);
                _syncLinqQueryReference = new WeakReference(_syncLinqQuery, true);
            }
        }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        public IBindableCollection<TResult> Results
        {
            get { return _syncLinqQuery; }
        }

        /// <summary>
        /// Gets or sets the LINQ query equivalent.
        /// </summary>
        public IEnumerable LinqEquivalent { get; set; }

        /// <summary>
        /// Gets the events raised by the result set.
        /// </summary>
        public CollectionEventCatcher Events
        {
            get { return _eventCatcher; }
        }

        #region IDisposable Members
        public void Dispose()
        {
            _eventCatcher.Dispose();
            _syncLinqQuery = null;
        }
        #endregion

        public bool IsQueryAlive()
        {
            return _syncLinqQueryReference.IsAlive;
        }
    }
}