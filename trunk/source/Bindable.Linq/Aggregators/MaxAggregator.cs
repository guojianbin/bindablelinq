using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Aggregators.Numerics;

namespace Bindable.Linq.Aggregators
{
    /// <summary>
    /// Aggregates a collection of numeric values into a bindable result, which will be updated when the source
    /// collection changes.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TAverageResult">The type of the average result.</typeparam>
    internal sealed class MaxAggregator<TValue, TAverageResult> : Aggregator<TValue, TValue>
    {
        private readonly INumeric<TValue, TAverageResult> _numericHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxAggregator&lt;TValue, TAverageResult&gt;"/> class.
        /// </summary>
        /// <param name="sourceCollection">The source collection.</param>
        /// <param name="numericHelper">A numeric helper.</param>
        public MaxAggregator(IBindableCollection<TValue> sourceCollection,
            INumeric<TValue, TAverageResult> numericHelper) : base(sourceCollection)
        {
            _numericHelper = numericHelper;
        }

        /// <summary>
        /// When overridden in a derived class, provides the aggregator the opportunity to calculate the
        /// value.
        /// </summary>
        protected override void AggregateOverride()
        {
            this.Current = _numericHelper.Max(this.SourceCollection);
        }
    }
}