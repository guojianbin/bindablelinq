using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Collections
{
    /// <summary>
    /// Implemented by bindable collections that allow clients to be notified before an item is yielded.
    /// </summary>
    public interface IBindableCollectionInterceptor : IBindableCollection
    {

    }

    /// <summary>
    /// Implemented by bindable collections that allow clients to be notified before an item is yielded.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public interface IBindableCollectionInterceptor<TElement> : IBindableCollection<TElement>, IBindableCollectionInterceptor
    {
        /// <summary>
        /// Adds the pre yield step.
        /// </summary>
        /// <param name="step">The step.</param>
        void AddPreYieldStep(Action<TElement> step);
    }
}
