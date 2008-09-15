using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Bindable.Linq.Helpers
{
    /// <summary>
    /// An internal cache for storing static <see cref="NotifyCollectionChangedEventArgs"/> instances.
    /// </summary>
    internal static class CollectionChangedCache
    {
        public static readonly NotifyCollectionChangedEventArgs Reset = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    }
}
