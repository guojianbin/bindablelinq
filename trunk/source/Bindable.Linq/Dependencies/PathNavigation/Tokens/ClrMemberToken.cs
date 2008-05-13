using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    /// <summary>
    /// A property monitor for CLR based properties.
    /// </summary>
    internal sealed class ClrMemberToken : MemberToken
    {
        private EventHandler<PropertyChangedEventArgs> _actualHandler;
        private readonly WeakEventReference<PropertyChangedEventArgs> _weakHandler;
        private readonly PropertyChangedEventHandler _weakHandlerWrapper;
        private IPropertyReader<object> _propertyReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClrPropertyMonitor"/> class.
        /// </summary>
        /// <param name="objectToObserve">The object to observe.</param>
        /// <param name="propertyName">The property path.</param>
        /// <param name="remainingPath">The remaining path.</param>
        /// <param name="callback">The callback.</param>
        public ClrMemberToken(object objectToObserve, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _actualHandler = CurrentTarget_PropertyChanged;
            _weakHandler = new WeakEventReference<PropertyChangedEventArgs>(_actualHandler);
            _weakHandlerWrapper = new PropertyChangedEventHandler(_weakHandler.WeakEventHandler);

            this.AcquireTarget(objectToObserve);
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to discard the current target.
        /// </summary>
        protected override void DiscardCurrentTargetOverride()
        {
            INotifyPropertyChanged currentTarget = this.CurrentTarget as INotifyPropertyChanged;
            if (currentTarget != null)
            {
                currentTarget.PropertyChanged -= _weakHandlerWrapper;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to monitor the current target.
        /// </summary>
        protected override void MonitorCurrentTargetOverride()
        {
            INotifyPropertyChanged currentTarget = this.CurrentTarget as INotifyPropertyChanged;
            if (currentTarget != null)
            {
                currentTarget.PropertyChanged += _weakHandlerWrapper;
            }
            _propertyReader = PropertyReaderFactory.Create<object>(this.CurrentTarget.GetType(), this.PropertyName);
        }

        /// <summary>
        /// When overriden in a derived class, gets the value of the current target object.
        /// </summary>
        /// <returns></returns>
        protected override object ReadCurrentPropertyValueOverride()
        {
            if (_propertyReader != null)
            {
                return _propertyReader.GetValue(this.CurrentTarget);
            }
            return null;
        }

        private void CurrentTarget_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.PropertyName)
            {
                this.HandleCurrentTargetPropertyValueChanged();
            }
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void DisposeOverride()
        {
            DiscardCurrentTargetOverride();
            if (_weakHandler != null)
            {
                _weakHandler.Dispose();
            }
        }
    }
}