using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq;
using Bindable.Linq.Collections;
using Bindable.Linq.Dependencies.Instances;
using Bindable.Linq.Configuration;
using Bindable.Linq.Dependencies.PathNavigation;

namespace Bindable.Linq.Dependencies.Definitions
{
    /// <summary>
    /// Defines a dependency on an external object that implements the INotifyPropertyChanged interface.
    /// </summary>
    public sealed class ExternalDependencyDefinition : IDependencyDefinition
    {
        private string _propertyPath;
        private object _targetObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalDependencyDefinition"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="targetObject">The target object.</param>
        public ExternalDependencyDefinition(string propertyPath, object targetObject)
        {
            _propertyPath = propertyPath;
            _targetObject = targetObject;
        }

        /// <summary>
        /// Gets or sets the property path.
        /// </summary>
        /// <value>The property path.</value>
        public string PropertyPath
        {
            get { return _propertyPath; }
            set { _propertyPath = value; }
        }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        /// <value>The target object.</value>
        public object TargetObject
        {
            get { return _targetObject; }
            set { _targetObject = value; }
        }

        /// <summary>
        /// Determines whether this instance can construct dependencies for a collection.
        /// </summary>
        /// <returns></returns>
        public bool AppliesToCollections()
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance can construct dependencies for a single element.
        /// </summary>
        /// <returns></returns>
        public bool AppliesToSingleElement()
        {
            return true;
        }

        /// <summary>
        /// Constructs the dependency for a collection of elements.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="sourceElements">The source elements.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        /// <returns></returns>
        public IDependency ConstructForCollection<TElement>(IBindableCollectionInterceptor<TElement> sourceElements, IPathNavigator pathNavigator)
        {
            return new ExternalDependency(this.TargetObject, this.PropertyPath, pathNavigator);
        }

        /// <summary>
        /// Constructs a dependency for a single element.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="pathNavigator">The path navigator.</param>
        /// <returns></returns>
        public IDependency ConstructForElement<TElement>(TElement sourceElement, IPathNavigator pathNavigator)
        {
            return new ExternalDependency(this.TargetObject, this.PropertyPath, pathNavigator);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: '{1}' on '{2}'", this.GetType().Name, this.PropertyPath, this.TargetObject.GetType().Name);
        }
    }
}