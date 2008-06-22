using System.ComponentModel;

namespace Bindable.Linq.Helpers
{
    /// <summary>
    /// An internal cache for storing static <see cref="PropertyChangedEventArgs"/> instances.
    /// </summary>
    internal static class PropertyChangedCache
    {
        public static readonly PropertyChangedEventArgs Count = new PropertyChangedEventArgs("Count");
        public static readonly PropertyChangedEventArgs IsLoading = new PropertyChangedEventArgs("IsLoading");
    }
}