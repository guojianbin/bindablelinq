using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq;

namespace Bindable.Linq.Aggregators
{
    /// <summary>
    /// An aggregator that finds the element at a given index within a collection.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class ElementAtAggregator<TElement> : Aggregator<TElement, TElement>
    {
        private int _index;
        private TElement _default;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAtAggregator&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        public ElementAtAggregator(IBindableCollection<TElement> source, int index)
            : base(source)
        {
            _index = index;
            _default = default(TElement);
        }

        /// <summary>
        /// When overridden in a derived class, provides the aggregator the opportunity to calculate the
        /// value.
        /// </summary>
        protected override void AggregateOverride()
        {
            int currentIndex = 0;
            bool found = false;
            TElement result = _default;
            foreach (TElement element in this.SourceCollection)
            {
                result = element;
                if (currentIndex == _index)
                {
                    found = true;
                    break;
                }
            }
            if (!found && currentIndex == -1)
            {
                result = _default;
            }
            this.Current = result;
        }
    }
}
