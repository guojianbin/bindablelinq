namespace Bindable.Linq.Tests.TestHelpers
{
    using System.Collections.Specialized;

    public static class CollectionChangeSpecifcationExtensions
    {
        public static CollectionChangeSpecification AsEvent(this NotifyCollectionChangedAction action)
        {
            var result = new CollectionChangeSpecification(action);
            return result;
        }

        public static CollectionChangeSpecification WithNewItems(this CollectionChangeSpecification spec, params object[] items)
        {
            spec.NewItems = items;
            return spec;
        }

        public static CollectionChangeSpecification WithOldItems(this CollectionChangeSpecification spec, params object[] items)
        {
            spec.OldItems = items;
            return spec;
        }

        public static CollectionChangeSpecification WithNewIndex(this CollectionChangeSpecification spec, int index)
        {
            spec.NewIndex = index;
            return spec;
        }

        public static CollectionChangeSpecification WithOldIndex(this CollectionChangeSpecification spec, int index)
        {
            spec.OldIndex = index;
            return spec;
        }

        public static CollectionChangeSpecification WithNewItemCount(this CollectionChangeSpecification spec, int count)
        {
            spec.NewItemsCount = count;
            return spec;
        }

        public static CollectionChangeSpecification WithOldItemCount(this CollectionChangeSpecification spec, int count)
        {
            spec.OldItemsCount = count;
            return spec;
        }

        public static CollectionChangeSpecification OnGroup(this CollectionChangeSpecification spec, int index)
        {
            spec.GroupIndex = index;
            return spec;
        }

        public static CollectionChangeSpecification WithNewItems(this NotifyCollectionChangedAction action, params object[] items)
        {
            return action.AsEvent().WithNewItems(items);
        }

        public static CollectionChangeSpecification WithNewItemCount(this NotifyCollectionChangedAction action, int count)
        {
            return action.AsEvent().WithNewItemCount(count);
        }

        public static CollectionChangeSpecification WithOldItemCount(this NotifyCollectionChangedAction action, int count)
        {
            return action.AsEvent().WithOldItemCount(count);
        }

        public static CollectionChangeSpecification WithOldItems(this NotifyCollectionChangedAction action, params object[] items)
        {
            return action.AsEvent().WithOldItems(items);
        }

        public static CollectionChangeSpecification WithNewIndex(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().WithNewIndex(index);
        }

        public static CollectionChangeSpecification WithOldIndex(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().WithOldIndex(index);
        }

        public static CollectionChangeSpecification OnGroup(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().OnGroup(index);
        }
    }
}