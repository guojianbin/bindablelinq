using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Bindable.Linq.Helpers;
using System.Reflection;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    /// <summary>
    /// A property monitor for CLR based properties.
    /// </summary>
    internal sealed class WindowsFormsMemberToken : MemberToken
    {
        private EventHandler _actualHandler;
        private IPropertyReader<object> _propertyReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsFormsMemberToken"/> class.
        /// </summary>
        /// <param name="objectToObserve">The object to observe.</param>
        /// <param name="propertyName">The property path.</param>
        /// <param name="remainingPath">The remaining path.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        public WindowsFormsMemberToken(object objectToObserve, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _actualHandler = CurrentTarget_PropertyChanged;
            
            this.AcquireTarget(objectToObserve);
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to discard the current target.
        /// </summary>
        protected override void DiscardCurrentTargetOverride()
        {
            if (this.CurrentTarget != null)
            {
                EventInfo eventInfo = this.CurrentTarget.GetType().GetEvent(this.PropertyName + "Changed");
                if (eventInfo != null)
                {
                    MethodInfo removeMethod = eventInfo.GetRemoveMethod();
                    if (removeMethod != null)
                    {
                        removeMethod.Invoke(this.CurrentTarget, new object[] { _actualHandler });
                    }
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to monitor the current target.
        /// </summary>
        protected override void MonitorCurrentTargetOverride()
        {
            if (this.CurrentTarget != null)
            {
                EventInfo eventInfo = this.CurrentTarget.GetType().GetEvent(this.PropertyName + "Changed");
                if (eventInfo != null)
                {
                    MethodInfo addMethod = eventInfo.GetAddMethod();
                    if (addMethod != null)
                    {
                        addMethod.Invoke(this.CurrentTarget, new object[] { _actualHandler });
                    }
                }
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

        private void CurrentTarget_PropertyChanged(object sender, EventArgs e)
        {
            this.HandleCurrentTargetPropertyValueChanged();
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected override void DisposeOverride()
        {
            DiscardCurrentTargetOverride();
        }
    }
}