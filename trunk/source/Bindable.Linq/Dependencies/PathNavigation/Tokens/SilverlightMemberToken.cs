using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bindable.Linq.Helpers;
using System.Windows;
using System.Reflection;

namespace Bindable.Linq.Dependencies.PathNavigation.Tokens
{
    /// <summary>
    /// A property monitor for WPF DependencyProperties.
    /// </summary>
    internal sealed class SilverlightMemberToken : MemberToken
    {
        private EventHandler _actualHandler;
        private DependencyProperty _dependencyProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SilverlightMemberToken"/> class.
        /// </summary>
        /// <param name="objectToObserve">The object to observe.</param>
        /// <param name="dependencyProperty">The dependency property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="remainingPath">The remaining path.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        public SilverlightMemberToken(DependencyObject objectToObserve, DependencyProperty dependencyProperty, string propertyName, string remainingPath, Action<object, string> callback, IPathNavigator pathNavigator)
            : base(objectToObserve, propertyName, remainingPath, callback, pathNavigator)
        {
            _dependencyProperty = dependencyProperty;
            _actualHandler = CurrentTarget_PropertyChanged;

            this.AcquireTarget(objectToObserve);
        }

        /// <summary>
        /// When overridden in a derived class, gives the class an opportunity to discard the current target.
        /// </summary>
        protected override void DiscardCurrentTargetOverride()
        {
            DependencyObject currentTarget = this.CurrentTarget as DependencyObject;
            if (currentTarget != null)
            {
                //var dpd = DependencyPropertyDescriptor.FromProperty(_dependencyProperty, currentTarget.GetType());
                //if (dpd != null)
                //{
                //    dpd.RemoveValueChanged(currentTarget, CurrentTarget_PropertyChanged);
                //}

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
            DependencyObject currentTarget = this.CurrentTarget as DependencyObject;
            if (currentTarget != null)
            {
                //var dpd = DependencyPropertyDescriptor.FromProperty(_dependencyProperty, currentTarget.GetType());
                //if (dpd != null)
                //{
                //    dpd.AddValueChanged(currentTarget, CurrentTarget_PropertyChanged);
                //}

                EventInfo eventInfo = this.CurrentTarget.GetType().GetEvent(this.PropertyName + "Changed");
                if (eventInfo != null)
                {
                    MethodInfo addMethod = eventInfo.GetAddMethod();
                    if (addMethod != null)
                    {
                        ParameterInfo pi = addMethod.GetParameters()[0];

                        Delegate d = Delegate.CreateDelegate(pi.ParameterType, this, this.GetType().GetMethod("CurrentTarget_PropertyChanged", BindingFlags.Public | BindingFlags.Instance));
                        addMethod.Invoke(this.CurrentTarget, new object[] { d });
                    }
                }
            }
        }

        /// <summary>
        /// When overriden in a derived class, gets the value of the current target object.
        /// </summary>
        /// <returns></returns>
        protected override object ReadCurrentPropertyValueOverride()
        {
            if (_dependencyProperty != null && this.CurrentTarget != null)
            {
                return ((DependencyObject)this.CurrentTarget).GetValue(_dependencyProperty);
            }
            return null;
        }

        public void CurrentTarget_PropertyChanged(object sender, EventArgs e)
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
