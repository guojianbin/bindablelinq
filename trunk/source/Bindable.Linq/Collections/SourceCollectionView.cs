using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Collections
{
    internal sealed class SourceCollectionView<TElement> : BindableCollection<SourceElementDescriptor<TElement>>
    {
        private IEnumerable<TElement> _originalCollection;
        private List<TElement> _duplicatedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceCollectionView&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="originalCollection">The original collection.</param>
        public SourceCollectionView(IEnumerable<TElement> originalCollection)
        {
            _originalCollection = originalCollection;
            
        }
    }
}
