using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq;

namespace Bindable.Linq.Aggregators
{
    /// <summary>
    /// Aggregates a source collection using a custom accumulator callback provided by the caller.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TAccumulate">The type of the accumulate.</typeparam>
    internal sealed class CustomAggregator<TSource, TAccumulate> : Aggregator<TSource, TAccumulate>
    {
        private readonly Func<TAccumulate, TSource, TAccumulate> _aggregator;
        private readonly TAccumulate _seed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAggregator&lt;TSource, TAccumulate&gt;"/> class.
        /// </summary>
        public CustomAggregator(IBindableCollection<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> aggregator) : base(source)
        {
            _aggregator = aggregator;
            _seed = seed;
        }

        /// <summary>
        /// When overridden in a derived class, provides the aggregator the opportunity to calculate the
        /// value.
        /// </summary>
        protected override void AggregateOverride()
        {
            TAccumulate result = _seed;
            foreach (TSource sourceItem in this.SourceCollection)
            {
                result = _aggregator(result, sourceItem);
            }
            this.Current = result;
        }
    }
}