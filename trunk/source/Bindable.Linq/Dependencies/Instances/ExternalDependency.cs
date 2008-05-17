using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Helpers;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.Instances
{
    /// <summary>
    /// Represents an item property dependency applied over a collection of items.
    /// </summary>
    internal sealed class ExternalDependency : IDependency
    {
        private readonly LockScope _dependencyLock = new LockScope();
        private readonly IToken _rootMonitor;
        private Action<object> _elementChangedCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalDependency"/> class.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        public ExternalDependency(object targetObject, string propertyPath, IPathNavigator pathNavigator)
        {
            _rootMonitor = pathNavigator.TraverseNext(targetObject, propertyPath, Element_PropertyChanged);
        }

        /// <summary>
        /// Called when a property on an element changes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="propertyPath">The property path.</param>
        private void Element_PropertyChanged(object element, string propertyPath)
        {
            Action<object> action = _elementChangedCallback;
            if (action != null)
            {
                action(element);
            }
        }

        /// <summary>
        /// Sets the callback action the dependency should invoke when the dependent object has a property that changes.
        /// </summary>
        /// <param name="action">The callback action to invoke.</param>
        /// <remarks>External objects require the entire query to be re-evaluated, so this method is ignored.</remarks>
        public void SetReevaluateElementCallback(Action<object, string> action)
        {
        }
        
        /// <summary>
        /// Sets the callback action the dependency should invoke when the dependent object changes, signalling the
        /// whole collection should re-evaluate.
        /// </summary>
        /// <param name="action">The callback action to invoke.</param>
        public void SetReevaluateCallback(Action<object> action)
        {
            _elementChangedCallback = action;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _rootMonitor.Dispose();
            _dependencyLock.Leave();
        }
    }
}
