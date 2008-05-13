using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Eventing
{
    /// <summary>
    /// Represents the different collection changed event models supported by Bindable LINQ.
    /// </summary>
    internal enum CollectionChangeMode
    {
        /// <summary>
        /// IBindingList.ListChanged support. Used predominantly by Windows Forms.
        /// </summary>
        ListChanged,

        /// <summary>
        /// INotifyCollectionChanged.CollectionChanged support. Used by Windows Presentation Foundation 
        /// and Silverlight, and is the lingua-franca for Bindable LINQ.
        /// </summary>
        CollectionChanged
    }
}
