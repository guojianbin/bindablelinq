using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Samples.MessengerClient.Helpers
{
    /// <summary>
    /// Contains helpful extension methods used internally.
    /// </summary>
    internal static class InnerExtensions
    {
        private static Random _random = new Random();

        /// <summary>
        /// Selects a random element from a list of elements.
        /// </summary>
        public static TElement SelectRandom<TElement>(this IEnumerable<TElement> elements)
        {
            int index = _random.Next(0, elements.Count());
            return elements.ElementAt(index);
        }
    }
}
