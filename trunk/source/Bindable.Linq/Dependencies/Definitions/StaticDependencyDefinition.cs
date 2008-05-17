using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bindable.Linq;
using Bindable.Linq.Collections;
using Bindable.Linq.Dependencies.PathNavigation;

namespace Bindable.Linq.Dependencies.Definitions
{
    /// <summary>
    /// Defines a dependency on a static property or member.
    /// </summary>
    public sealed class StaticDependencyDefinition : IDependencyDefinition
    {
        private MemberInfo _member;
        private string _propertyPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticDependencyDefinition"/> class.
        /// </summary>
        /// <param name="propertyPath">The property path.</param>
        /// <param name="member">The member.</param>
        public StaticDependencyDefinition(string propertyPath, MemberInfo member)
        {
            _member = member;
            _propertyPath = propertyPath;
        }

        /// <summary>
        /// Gets or sets the member.
        /// </summary>
        /// <value>The member.</value>
        public MemberInfo Member
        {
            get { return _member; }
            set { _member = value; }
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
            return false;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}: '{1}' on '{2}'", this.GetType().Name, this.PropertyPath, this.Member.DeclaringType.Name);
        }
    }
}