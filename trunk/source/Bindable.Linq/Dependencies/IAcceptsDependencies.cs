using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Dependencies
{
    /// <summary>
    /// Implemented by SyncLINQ operations which can have dependencies.
    /// </summary>
    public interface IAcceptsDependencies
    {
        /// <summary>
        /// Sets a new dependency on a SyncLINQ operation.
        /// </summary>
        /// <param name="definition">A definition of the dependency.</param>
        void AcceptDependency(IDependencyDefinition definition);
    }
}