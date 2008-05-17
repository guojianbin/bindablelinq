using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    /// <summary>
    /// A base class for objects that monitor a property.
    /// </summary>
    internal abstract class MemberToken : IToken
    {
        private readonly LockScope _propertyMonitorLock = new LockScope();
        private Action<object, string> _changeDetectedCallback;
        private object _currentTarget;
        private IPathNavigator _pathNavigator;
        private IToken _nextMonitor;
        private string _remainingPath;
        private string _propertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberToken"/> class.
        /// </summary>
        /// <param name="currentTarget">The current target.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="remainingPath">The remaining path.</param>
        /// <param name="changeDetectedCallback">The change detected callback.</param>
        /// <param name="traverser">The traverser.</param>
        public MemberToken(object currentTarget, string propertyName, string remainingPath, Action<object, string> changeDetectedCallback, IPathNavigator traverser) 
        {
            _changeDetectedCallback = changeDetectedCallback;
            _remainingPath = remainingPath;
            _propertyName = propertyName;
            _pathNavigator = traverser;
        }

        /// <summary>
        /// Gets the remaining fragments.
        /// </summary>
        public string RemainingPath
        {
            get { return _remainingPath; }
        }

        /// <summary>
        /// Gets the remaining fragments.
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Gets the current target.
        /// </summary>
        protected object CurrentTarget
        {
            get { return _currentTarget; }
        }

        /// <summary>
        /// Gets the property monitor lock.
        /// </summary>
        protected LockScope PropertyMonitorLock
        {
            get { return _propertyMonitorLock; }
        }

        /// <summary>
        /// Gets the next monitor.
        /// </summary>
        public IToken NextToken
        {
            get { return _nextMonitor; }
            private set
            {
                if (_nextMonitor != null)
                {
                    _nextMonitor.Dispose();
                }
                _nextMonitor = value;
            }
        }

        /// <summary>
        /// Gets the traverser.
        /// </summary>
        protected IPathNavigator PathNavigator
        {
            get { return _pathNavigator; }
        }

        /// <summary>
        /// Acquires the target.
        /// </summary>
        /// <param name="target">The target.</param>
        public void AcquireTarget(object target)
        {
            using (this.PropertyMonitorLock.Enter(this))
            {
                if (this.CurrentTarget != null)
                {
                    this.DiscardCurrentTargetOverride();
                }
                _currentTarget = target;
                if (this.CurrentTarget != null)
                {
                    this.MonitorCurrentTargetOverride();
                    this.NextToken = this.PathNavigator.TraverseNext(ReadCurrentPropertyValueOverride(), _remainingPath, NextMonitor_ChangeDetected);
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to discard the current target.
        /// </summary>
        protected abstract void DiscardCurrentTargetOverride();

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to monitor the current target.
        /// </summary>
        protected abstract void MonitorCurrentTargetOverride();

        /// <summary>
        /// When overriden in a derived class, gets the value of the current target object.
        /// </summary>
        /// <returns></returns>
        protected abstract object ReadCurrentPropertyValueOverride();

        /// <summary>
        /// When overridden in a derived class, lets the derived class dispose any event handlers.
        /// </summary>
        protected abstract void DisposeOverride();

        private void NextMonitor_ChangeDetected(object changedObject, string propertyName)
        {
            this.ChangeDetected(_propertyName + "." + propertyName);
        }

        /// <summary>
        /// Handles the current target property value changed.
        /// </summary>
        protected void HandleCurrentTargetPropertyValueChanged()
        {
            using (this.PropertyMonitorLock.Enter(this))
            {
                object newValue = ReadCurrentPropertyValueOverride();
                this.NextToken = this.PathNavigator.TraverseNext(newValue, _remainingPath, NextMonitor_ChangeDetected);
            }
            this.ChangeDetected(_propertyName);
        }

        /// <summary>
        /// Notifies the parent IPropertyMonitor that a property on the target object has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void ChangeDetected(string propertyName)
        {
            if (_changeDetectedCallback != null)
            {
                _changeDetectedCallback(_currentTarget, propertyName);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.DisposeOverride();
            this.NextToken = null;
        }
    }
}
