using System;
using System.Collections.Generic;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Tests.TestLanguage.Specifications
{
    /// <summary>
    /// Represents a specification with a title and a known input type, but no specified 
    /// output type yet. 
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    internal class PartialSpecification<TInput>
    {
        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialTypedSpecification&lt;TInput&gt;"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public PartialSpecification(string title)
        {
            _title = title;
        }

        /// <summary>
        /// Specifies the Bindable LINQ query, resulting in a specification with known input and output types.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="queryCreator">The query creator.</param>
        /// <returns></returns>
        public FullyTypedSpecification<TInput, TResult> UsingBindableLinq<TResult>(Func<IEnumerable<TInput>, IBindableCollection<TResult>> queryCreator)
        {
            return new FullyTypedSpecification<TInput, TResult>(_title, queryCreator);
        }
    }
}
