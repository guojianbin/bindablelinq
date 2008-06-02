using System;

namespace Bindable.Linq.Samples.MessengerClient.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Contains helpful extension methods used internally.
    /// </summary>
    internal static class InnerExtensions
    {
        private static readonly Random _random = new Random();

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