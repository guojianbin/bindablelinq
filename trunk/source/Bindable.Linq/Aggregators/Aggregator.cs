﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bindable.Linq;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Configuration;

namespace Bindable.Linq.Aggregators
{
    /// <summary>
    /// Serves as a base class for all aggregate functions. From Bindable LINQ's perspective,
    /// an <see cref="T:Aggregator`2"/> is a LINQ operation which tranforms a collection of items
    /// into an item. This makes it different to an <see cref="T:Iterator`2"/> which
    /// transforms a collection into another collection, or an <see cref="T:Operator`2"/>
    /// which transforms one item into another.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class Aggregator<TSource, TResult> : 
        IBindable<TResult>,
        IConfigurable,
        IDisposable
    {
        private static readonly PropertyChangedEventArgs CurrentPropertyChangedEventArgs = new PropertyChangedEventArgs("Current");
        private readonly LockScope _aggregatorLock = new LockScope();
        private readonly StateScope _calculatingState;
        private readonly CollectionChangeObserver _collectionChangedObserver;
        private readonly IBindableCollection<TSource> _sourceCollection;
        private bool _isSourceCollectionLoaded;
        private TResult _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Aggregator&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        public Aggregator(IBindableCollection<TSource> sourceCollection)
        {
            _collectionChangedObserver = new CollectionChangeObserver(SourceCollection_CollectionChanged);
            _calculatingState = new StateScope();
            _sourceCollection = sourceCollection;
            _collectionChangedObserver.Attach(sourceCollection);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the lock object on this aggregate.
        /// </summary>
        protected LockScope AggregateLock
        {
            get { return _aggregatorLock; }
        }

        /// <summary>
        /// Gets a state scope that can be entered to indicate the aggregate is being calculated.
        /// </summary>
        protected StateScope CalculatingState
        {
            get { return _calculatingState; }
        }

        /// <summary>
        /// Gets the source collections that this aggregate is aggregating.
        /// </summary>
        protected IBindableCollection<TSource> SourceCollection
        {
            get { return _sourceCollection; }
        }

        /// <summary>
        /// The resulting value. Rather than being returned directly, the value is housed
        /// within the <see cref="T:IBindableElement`1"/> container so that it can be updated when
        /// the source it was created from changes.
        /// </summary>
        /// <value></value>
        public TResult Current
        {
            get
            {
                EnsureLoaded();
                return _value;
            }
            protected set
            {
                bool valueChanged = false;
                using (this.AggregateLock.Enter(this))
                {
                    if (!Equals(_value, value))
                    {
                        _value = value;
                        valueChanged = true;
                    }
                }
                if (valueChanged)
                {
                    OnPropertyChanged(CurrentPropertyChangedEventArgs);
                }
            }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IBindingConfiguration Configuration
        {
            get
            {
                IBindingConfiguration result = BindingConfigurations.Default;
                if (this.SourceCollection is IConfigurable)
                {
                    result = ((IConfigurable)this.SourceCollection).Configuration;
                }
                return result;
            }
        }

        /// <summary>
        /// Refreshes the value by forcing it to be recalculated or reconsidered.
        /// </summary>
        public void Refresh()
        {
            this.Aggregate();
        }

        /// <summary>
        /// When overridden in a derived class, provides the aggregator the opportunity to calculate the 
        /// value.
        /// </summary>
        protected abstract void AggregateOverride();

        private void Aggregate()
        {
            using (this.CalculatingState.Enter())
            {
                AggregateOverride();
            }
        }

        private void EnsureLoaded()
        {
            bool calculationNeeded = false;

            using (this.AggregateLock.Enter(this))
            {
                if (_isSourceCollectionLoaded == false)
                {
                    _isSourceCollectionLoaded = true;
                    calculationNeeded = true;
                }
            }

            if (calculationNeeded)
            {
                Aggregate();
            }
        }

        private void SourceCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _calculatingState.Leave();
            GC.SuppressFinalize(this);
        }
    }
}