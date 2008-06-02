using System;

namespace Bindable.Linq.Tests.TestHelpers
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Text;
    using Helpers;
    using NUnit.Framework;

    public class CollectionChangeSpecification
    {
        public CollectionChangeSpecification(NotifyCollectionChangedAction action)
        {
            Action = action;
        }

        public NotifyCollectionChangedAction Action { get; set; }

        public object[] OldItems { get; set; }

        public object[] NewItems { get; set; }

        public int? NewIndex { get; set; }

        public int? OldIndex { get; set; }

        public int? NewItemsCount { get; set; }

        public int? OldItemsCount { get; set; }

        public string Description
        {
            get
            {
                string result = Action.ToString();
                if (GroupIndex != null)
                {
                    result += " on group " + GroupIndex.Value;
                }
                if (NewItems != null)
                {
                    result += ", new items " + PrintItems(NewItems);
                }
                if (OldItems != null)
                {
                    result += ", old items " + PrintItems(OldItems);
                }
                if (NewIndex != null)
                {
                    result += ", new index " + NewIndex;
                }
                if (OldIndex != null)
                {
                    result += ", original index " + OldIndex;
                }
                return result;
            }
        }

        public int? GroupIndex { get; set; }

        private static bool CompareItems(IEnumerable actualItems, IEnumerable expectedItems)
        {
            IEnumerable lhs = actualItems ?? new object[0];
            IEnumerable rhs = expectedItems ?? new object[0];

            IEnumerator lhsEnumerator = lhs.GetEnumerator();
            IEnumerator rhsEnumerator = rhs.GetEnumerator();

            while (true)
            {
                bool lhsHasNext = lhsEnumerator.MoveNext();
                bool rhsHasNext = rhsEnumerator.MoveNext();

                if (lhsHasNext == false && rhsHasNext == false)
                {
                    return true;
                }
                else if (lhsHasNext != rhsHasNext)
                {
                    return false;
                }
                else if (!Equals(lhsEnumerator.Current, rhsEnumerator.Current))
                {
                    return false;
                }
            }
        }

        private static string PrintItems(IEnumerable items)
        {
            var builder = new StringBuilder();
            if (items != null)
            {
                builder.Append("{ ");
                foreach (object item in items)
                {
                    if (item != null)
                    {
                        builder.Append("\"").Append(item.ToString()).Append("\", ");
                    }
                }
                if (builder.Length > 0)
                {
                    builder.Remove(builder.Length - 2, 2);
                }
                builder.Append(" }");
            }
            return builder.ToString();
        }

        public void CompareTo(NotifyCollectionChangedEventArgs argument)
        {
            if (argument.Action != Action)
            {
                Assert.Fail("Expected action {0} but got action {1} instead.".FormatWith(Action, argument.Action));
            }

            if (OldItems != null)
            {
                if (!CompareItems(argument.OldItems, OldItems))
                {
                    Assert.Fail("Actual old items '{0}' do not match expected items '{1}'", PrintItems(argument.OldItems), PrintItems(OldItems));
                }
            }
            if (NewItems != null)
            {
                if (!CompareItems(argument.NewItems, NewItems))
                {
                    Assert.Fail("Actual new items '{0}' do not match expected items '{1}'", PrintItems(argument.NewItems), PrintItems(NewItems));
                }
            }
            if (NewItemsCount != null)
            {
                Assert.AreEqual(NewItemsCount.Value, argument.NewItems.Count, "Actual new items '{0}' were not the correct number of items '{1}'", PrintItems(argument.NewItems), NewItemsCount.Value);
            }
            if (OldItemsCount != null)
            {
                Assert.AreEqual(OldItemsCount.Value, argument.OldItems.Count, "Actual old items '{0}' were not the correct number of items '{1}'", PrintItems(argument.OldItems), OldItemsCount.Value);
            }

            switch (Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (NewIndex != null)
                    {
                        Assert.AreEqual(NewIndex.Value, argument.NewStartingIndex, "NewStartingIndex for Add event was not as expected.");
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (NewIndex != null)
                    {
                        Assert.AreEqual(NewIndex.Value, argument.NewStartingIndex, "NewStartingIndex for Move event was not as expected.");
                    }
                    if (OldIndex != null)
                    {
                        Assert.AreEqual(OldIndex.Value, argument.OldStartingIndex, "OldStartingIndex for Move event was not as expected.");
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (OldIndex != null)
                    {
                        Assert.AreEqual(OldIndex.Value, argument.OldStartingIndex, "OldStartingIndex for Remove event was not as expected.");
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (OldIndex != null)
                    {
                        Assert.AreEqual(OldIndex.Value, argument.OldStartingIndex, "OldStartingIndex for Replace event was not as expected.");
                    }
                    break;
            }
        }
    }
}