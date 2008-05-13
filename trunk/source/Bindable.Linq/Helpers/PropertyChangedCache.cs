using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Bindable.Linq.Helpers
{
    internal static class PropertyChangedCache
    {
        public static readonly PropertyChangedEventArgs Count = new PropertyChangedEventArgs("Count");
        public static readonly PropertyChangedEventArgs IsLoading = new PropertyChangedEventArgs("IsLoading");
    }
}
