using System;

namespace Bindable.Linq.Tests.TestHelpers
{
    using System.Collections;
    using Collections;

    /// <summary>
    /// A helper class for writing Bindable LINQ unit tests.
    /// </summary>
    internal static class Given
    {
        public static TCollection ExistingCollection<TCollection>(TCollection inputs) where TCollection : IEnumerable
        {
            return inputs;
        }

        public static BindableCollection<TInput> Collection<TInput>(params TInput[] inputs)
        {
            var results = new BindableCollection<TInput>();
            results.AddRange(inputs);
            return results;
        }
    }
}