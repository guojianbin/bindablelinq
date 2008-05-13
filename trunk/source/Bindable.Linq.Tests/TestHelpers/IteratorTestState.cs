using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;
using Bindable.Linq.Helpers;
using Bindable.Linq.Collections;

namespace Bindable.Linq.Tests.TestHelpers
{
    /// <summary>
    /// This class acts as a helper for testing Bindable LINQ queries against their LINQ counterparts.
    /// </summary>
    /// <typeparam name="TElement">The type of the element returned by the queries.</typeparam>
    internal class IteratorTestState<TInput, TResult> : IDisposable
    {
        private IEnumerable<TInput> _inputs;
        private Stack<IDependencyDefinition> _dependencies = new Stack<IDependencyDefinition>();
        private IBindableCollection<TResult> _syncLinqQuery;
        private WeakReference _syncLinqQueryReference;
        private IEnumerable _linqEquivalent;
        private CollectionEventCatcher _eventCatcher;
        private CompatibilityExpectation _compatibilityExpectation;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTester&lt;TElement&gt;"/> class.
        /// </summary>
        public IteratorTestState()
        {

        }

        /// <summary>
        /// Gets or sets whether or not order is important when comparing the Bindable LINQ query to the LINQ 
        /// query.
        /// </summary>
        public CompatibilityExpectation CompatibilityExpectation
        {
            get { return _compatibilityExpectation; }
            set { _compatibilityExpectation = value; }
        }

        /// <summary>
        /// Gets or sets the inputs.
        /// </summary>
        public IEnumerable<TInput> Inputs
        {
            get { return _inputs; }
            set { _inputs = value; }
        }

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
        public IEnumerable LinqEquivalent
        {
            get { return _linqEquivalent; }
            set { _linqEquivalent = value; }
        }

        /// <summary>
        /// Gets the events raised by the result set.
        /// </summary>
        public CollectionEventCatcher Events
        {
            get { return _eventCatcher; }
        }

        public bool IsQueryAlive()
        {
            return _syncLinqQueryReference.IsAlive;
        }

        public void Dispose()
        {
            _eventCatcher.Dispose();
            _syncLinqQuery = null;
        }
    }
}
