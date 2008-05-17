using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Bindable.Linq.Dependencies.PathNavigation.TokenFactories;
using Bindable.Linq.Dependencies.PathNavigation.Tokens;

namespace Bindable.Linq.Dependencies.PathNavigation
{
    /// <summary>
    /// A factory for the construction of property monitors by detecting information about the object.
    /// </summary>
    public class PathNavigator : IPathNavigator
    {
        private ITokenFactory[] _traversers;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathNavigator"/> class.
        /// </summary>
        /// <param name="traversers">The traversers.</param>
        public PathNavigator(params ITokenFactory[] traversers)
        {
            _traversers = traversers;
        }

        /// <summary>
        /// Creates an appropriate property monitor for the remaining property path string on the target object.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>
        /// An appropriate <see cref="IToken"/> for the property.
        /// </returns>
        public IToken TraverseNext(object target, string propertyPath, Action<object,string> callback)
        {
            propertyPath = propertyPath ?? string.Empty;
            IToken result = null;
            foreach (ITokenFactory traverser in _traversers)
            {
                result = traverser.ParseNext(target, propertyPath, callback, this);
                if (result != null)
                {
                    break;
                }
            }
            return result;
        }
    }
}
