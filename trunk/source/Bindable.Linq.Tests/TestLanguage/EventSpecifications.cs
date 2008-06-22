using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Tests.TestLanguage.Expectations;
using System.Collections.Specialized;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Extension methods that can be applied to <see cref="NotifyCollectionChangedAction"/> instances.
    /// </summary>
    internal static class EventSpecifications
    {
        public static RaiseEventExpectation AsEvent(this NotifyCollectionChangedAction action)
        {
            var result = new RaiseEventExpectation() { Action = action };
            return result;
        }

        public static RaiseEventExpectation WithNew(this RaiseEventExpectation spec, params object[] items)
        {
            spec.NewItems = items;
            return spec;
        }

        public static RaiseEventExpectation WithOld(this RaiseEventExpectation spec, params object[] items)
        {
            spec.OldItems = items;
            return spec;
        }

        public static RaiseEventExpectation AtNew(this RaiseEventExpectation spec, int index)
        {
            spec.NewIndex = index;
            return spec;
        }

        public static RaiseEventExpectation AtOld(this RaiseEventExpectation spec, int index)
        {
            spec.OldIndex = index;
            return spec;
        }

        public static RaiseEventExpectation WithNewCount(this RaiseEventExpectation spec, int count)
        {
            spec.NewItemsCount = count;
            return spec;
        }

        public static RaiseEventExpectation WithOldCount(this RaiseEventExpectation spec, int count)
        {
            spec.OldItemsCount = count;
            return spec;
        }

        public static RaiseEventExpectation With(this RaiseEventExpectation spec, params object[] items)
        {
            switch (spec.Action.Value)
            {
                case NotifyCollectionChangedAction.Add:
                    return spec.WithNew(items);
                case NotifyCollectionChangedAction.Remove:
                    return spec.WithOld(items);
                case NotifyCollectionChangedAction.Move:
                    return spec.WithOld(items);
                default:
                    throw new NotSupportedException("Invalid event specification: Both New and Old items must be specified explicitly when not using Add, Remove or Move event specifications.");
            }
        }

        public static RaiseEventExpectation With(this NotifyCollectionChangedAction action, params object[] items)
        {
            return action.AsEvent().With(items);
        }

        public static RaiseEventExpectation WithNew(this NotifyCollectionChangedAction action, params object[] items)
        {
            return action.AsEvent().WithNew(items);
        }

        public static RaiseEventExpectation WithNewCount(this NotifyCollectionChangedAction action, int count)
        {
            return action.AsEvent().WithNewCount(count);
        }

        public static RaiseEventExpectation WithOldCount(this NotifyCollectionChangedAction action, int count)
        {
            return action.AsEvent().WithOldCount(count);
        }

        public static RaiseEventExpectation WithOld(this NotifyCollectionChangedAction action, params object[] items)
        {
            return action.AsEvent().WithOld(items);
        }

        public static RaiseEventExpectation AtNew(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().AtNew(index);
        }

        public static RaiseEventExpectation AtOld(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().AtOld(index);
        }

        public static RaiseEventExpectation At(this NotifyCollectionChangedAction action, int index)
        {
            return action.AsEvent().At(index);
        }

        public static RaiseEventExpectation At(this RaiseEventExpectation spec, int index)
        {
            switch (spec.Action.Value)
            {
                case NotifyCollectionChangedAction.Add:
                    return spec.AtNew(index);
                case NotifyCollectionChangedAction.Remove:
                    return spec.AtNew(index);
                default:
                    throw new NotSupportedException("Invalid event specification: Both New and Old indexes must be specified explicitly when not using Add or Remove event specifications.");
            }
        }
    }
}