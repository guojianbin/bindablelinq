using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Helpers;
using Bindable.Linq.Tests.TestObjectModel;
using Bindable.Linq.Collections;
using System.Collections;

namespace Bindable.Linq.Tests.TestHelpers
{
    /// <summary>
    /// A helper class for writing Bindable LINQ unit tests.
    /// </summary>
    internal static class Given
    {
        public static TCollection ExistingCollection<TCollection>(TCollection inputs) 
            where TCollection : IEnumerable
        {
            return inputs;
        }
        
        public static BindableCollection<TInput> Collection<TInput>(params TInput[] inputs)
        {
            BindableCollection<TInput> results = new BindableCollection<TInput>();
            results.AddRange(inputs);
            return results;
        }

    }
}
