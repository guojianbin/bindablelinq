using System.Collections.Generic;
using System.Linq;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class UnionIterator<TElement> : Iterator<IBindableCollection<TElement>, TElement> where TElement : class
    {
        private ElementActioner<IBindableCollection<TElement>> _rootActioner;
        private Dictionary<IBindableCollection<TElement>, ElementActioner<TElement>> _childCollectionActioners;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnionIterator&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="elements">The elements.</param>
        public UnionIterator(IBindableCollection<IBindableCollection<TElement>> elements)
            : base(elements)
        {
            _childCollectionActioners = new Dictionary<IBindableCollection<TElement>, ElementActioner<TElement>>();

            _rootActioner = new ElementActioner<IBindableCollection<TElement>>(SourceCollection,
                item => ChildCollectionAdded(item),
                item => ChildCollectionRemoved(item));
        }

        private void ChildCollectionRemoved(IBindableCollection<TElement> collection)
        {
        }

        private void ChildCollectionAdded(IBindableCollection<TElement> collection)
        {
            _childCollectionActioners[collection] = new ElementActioner<TElement>(
                collection,
                item => ResultCollection.Add(item),
                item => ResultCollection.Remove(item)
                );
        }

        protected override void LoadSourceCollection()
        {
            SourceCollection.Evaluate();
        }

        protected override void ReactToAddRange(int sourceStartingIndex, IEnumerable<IBindableCollection<TElement>> addedItems)
        {
        }

        protected override void ReactToMoveRange(int sourceStartingIndex, IEnumerable<IBindableCollection<TElement>> movedItems)
        {   
        }

        protected override void ReactToRemoveRange(IEnumerable<IBindableCollection<TElement>> removedItems)
        {
        }

        protected override void ReactToReplaceRange(IEnumerable<IBindableCollection<TElement>> oldItems, IEnumerable<IBindableCollection<TElement>> newItems)
        {
        }

        protected override void ReactToItemPropertyChanged(IBindableCollection<TElement> item, string propertyName)
        {
        }

        protected override void ResetOverride()
        {
            _childCollectionActioners.Clear();
        }
    }
}