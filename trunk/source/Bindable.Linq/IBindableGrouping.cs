using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace Bindable.Linq
{
    /// <summary>
    /// Implemented by classes that represent a bindable collection of objects with a common key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public interface IBindableGrouping<TKey, TElement> : 
        IGrouping<TKey, TElement>, 
        IBindableCollection<TElement>, 
        INotifyCollectionChanged
    {
    }
}