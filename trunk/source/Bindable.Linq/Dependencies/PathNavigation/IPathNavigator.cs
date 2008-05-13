﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation
{
    /// <summary>
    /// An interface implemented by classes which can traverse a property path.
    /// </summary>
    public interface IPathNavigator
    {
        /// <summary>
        /// Creates an appropriate property monitor for the remaining property path string on the target object.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>An appropriate <see cref="IPropertyMonitor"/> for the property.</returns>
        IToken TraverseNext(object target, string propertyPath, Action<object, string> callback);
    }
}