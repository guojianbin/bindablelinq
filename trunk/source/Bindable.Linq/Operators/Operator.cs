using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bindable.Linq;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Configuration;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Serves as a base class for all operator functions. From Bindable LINQ's perspective,
    /// an <see cref="T:Operator`2"/> is a LINQ operation which tranforms a single source items
    /// into single result item. This makes it different to an <see cref="T:Iterator`2"/> which
    /// transforms a collection into another collection, or an <see cref="T:Aggregator`2"/>
    /// which transforms a collection into a single element.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class Operator<TSource, TResult> : 
        IBindable<TResult>,
        IConfigurable,
        IAcceptsDependencies
    {
        private static readonly PropertyChangedEventArgs CurrentPropertyChangedEventArgs = new PropertyChangedEventArgs("Current");
        private readonly LockScope _operatorLock = new LockScope(); 
        private readonly IBindable<TSource> _source;
        private readonly List<IDependency> _dependencies;
        private readonly PropertyChangeObserver _sourcePropertyChangeObserver;
        private readonly EventHandler<PropertyChangedEventArgs> _eventHandler;
        private TResult _current;
        private bool _isSourceLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="Operator&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public Operator(IBindable<TSource> source)
        {
            _dependencies = new List<IDependency>();
            _eventHandler = Source_PropertyChanged;
            _sourcePropertyChangeObserver = new PropertyChangeObserver(_eventHandler);
            _source = source;
            _sourcePropertyChangeObserver.Attach(_source);
        }

        /// <summary>
        /// Gets the operator lock.
        /// </summary>
        /// <value>The operator lock.</value>
        protected LockScope OperatorLock
        {
            get { return _operatorLock; }
        }

        /// <summary>
        /// The resulting value. Rather than being returned directly, the value is housed
        /// within the <see cref="T:IBindable`1"/> container so that it can be updated when
        /// the source it was created from changes.
        /// </summary>
        /// <value></value>
        public TResult Current
        {
            get 
            {
                EnsureLoaded();
                return _current; 
            }
            set 
            {
                _current = value;
                OnPropertyChanged(CurrentPropertyChangedEventArgs);
            }
        }

        private void EnsureLoaded()
        {
            bool refreshNeeded = false;

            using (this.OperatorLock.Enter(this))
            {
                if (_isSourceLoaded == false)
                {
                    _isSourceLoaded = true;
                    refreshNeeded = true;
                }
            }

            if (refreshNeeded)
            {
                Refresh();
            }
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        public IBindable<TSource> Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IBindingConfiguration Configuration
        {
            get
            {
                IBindingConfiguration result = BindingConfigurations.Default;
                if (this.Source is IConfigurable)
                {
                    result = ((IConfigurable)this.Source).Configuration;
                }
                return result;
            }
        }

        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// When overridden in a derived class, refreshes the object.
        /// </summary>
        protected abstract void RefreshOverride();

        /// <summary>
        /// Refreshes the object.
        /// </summary>
        public void Refresh()
        {
            this.RefreshOverride();
        }

        /// <summary>
        /// Sets a new dependency on a Bindable LINQ operation.
        /// </summary>
        /// <param name="definition">A definition of the dependency.</param>
        public void AcceptDependency(IDependencyDefinition definition)
        {
            if (definition.AppliesToSingleElement())
            {
                IDependency dependency = definition.ConstructForElement(_source, this.Configuration.CreatePathNavigator());
                dependency.SetReevaluateCallback(o => Refresh());
                _dependencies.Add(dependency);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangingEventArgs"/> instance containing the event data.</param>
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
            _sourcePropertyChangeObserver.Dispose();
            foreach (IDependency dependency in _dependencies)
            {
                dependency.Dispose();
            }
        }
    }
}