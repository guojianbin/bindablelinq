using System.Collections.Generic;
using Bindable.Linq.Interfaces.Internal.Events;
using System.ComponentModel;
using System;

namespace Bindable.Linq.Interfaces.Internal
{
    /// <summary>
    /// The internal representation of a bindable collection.
    /// </summary>
    internal interface IInternalCollection<TElement> : IEnumerable<TElement>, IRefreshable, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance has already evaluated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has evaluated; otherwise, <c>false</c>.
        /// </value>
        bool HasEvaluated { get; }

        /// <summary>
        /// Occurs when the collection is being evaluated (GetEnumerator() is called) for the first time, just before it returns 
        /// the results, to provide insight into the items being evaluated. This allows consumers to iterate the items in a collection 
        /// just before they are returned to the caller, while still enabling delayed execution of queries.
        /// </summary>
        event EvaluatingEventHandler<TElement> Evaluating;

        /// <summary>
        /// Occurs when one or more consecutive items are added to the collection.
        /// </summary>
        event AddedEventHandler<TElement> ItemsAdded;

        /// <summary>
        /// Occurs when one or more consecutive items are removed from the collection.
        /// </summary>
        event RemovedEventHandler<TElement> ItemsRemoved;

        /// <summary>
        /// Occurs when one or more items in the collection have been replaced.
        /// </summary>
        event ReplacedEventHandler<TElement> ItemsReplaced;

        /// <summary>
        /// Occurs when one or more items in the collection have been moved.
        /// </summary>
        event MovedEventHandler<TElement> ItemsMoved;
        
        /// <summary>
        /// Occurs when the collection has changed significantly and consumers should re-read the collection.
        /// </summary>
        event ResetEventHandler Reset;
    }
}