using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq
{
    /// <summary>
    /// Implemented by objects that provide a Refresh method.
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Refreshes the object.
        /// </summary>
        void Refresh();
    }
}