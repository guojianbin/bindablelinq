using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation.TokenFactories
{
    /// <summary>
    /// Implemented by factories that construct <see cref="IPropertyMonitor">IPropertyMonitors</see> from a property path.
    /// </summary>
    public interface ITokenFactory
    {
        /// <summary>
        /// Creates an appropriate property monitor for the remaining property path string on the target object.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>An appropriate <see cref="IPropertyMonitor"/> for the property.</returns>
        IToken ParseNext(object target, string propertyPath, Action<object, string> callback, IPathNavigator pathNavigator);
    }
}
