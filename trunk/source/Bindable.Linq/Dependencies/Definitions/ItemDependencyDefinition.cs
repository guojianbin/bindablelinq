using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq;
using Bindable.Linq.Collections;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Dependencies.PathNavigation;
using Bindable.Linq.Dependencies.Instances;

namespace Bindable.Linq.Dependencies.Definitions
{
    /// <summary>
    /// Defines a dependency on a property on an item where the item implements the INotifyPropertyChanged interface.
    /// </summary>
    public sealed class ItemDependencyDefinition : IDependencyDefinition
    {
        private string _parameterName;
        private string _propertyPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDependencyDefinition"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        public ItemDependencyDefinition(string propertyPath)
        {
            _propertyPath = propertyPath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDependencyDefinition"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public ItemDependencyDefinition(string propertyPath, string parameterName)
        {
            _propertyPath = propertyPath;
            _parameterName = parameterName;
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = value; }
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
            return false;
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
            return new ItemDependency<TElement>(this.PropertyPath, sourceElements, pathNavigator);
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
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: '{1}' on '{2}'", this.GetType().Name, this.PropertyPath, this.ParameterName);
        }
    }
}