using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// Projects one item to another item.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal sealed class ProjectOperator<TSource, TResult> : Operator<TSource, TResult>
    {
        private Func<TSource, TResult> _projector;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectOperator&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="projector">The projector.</param>
        public ProjectOperator(IBindable<TSource> source, Func<TSource, TResult> projector)
            : base(source)
        {
            _projector = projector;
        }

        /// <summary>
        /// When overridden in a derived class, refreshes the object.
        /// </summary>
        protected override void RefreshOverride()
        {
            TSource source = this.Source.Current;
            if (source != null)
            {
                this.Current = _projector(source);
            }
            else
            {
                this.Current = default(TResult);
            }
        }
    }
}