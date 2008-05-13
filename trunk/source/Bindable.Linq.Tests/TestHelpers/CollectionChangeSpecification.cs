using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Bindable.Linq.Helpers;
using NUnit.Framework;

namespace Bindable.Linq.Tests.TestHelpers
{
    public class CollectionChangeSpecification
    {
        private NotifyCollectionChangedAction _action;
        private object[] _oldItems;
        private object[] _newItems;
        private int? _newIndex;
        private int? _oldIndex;
        private int? _groupIndex;
        private int? _newItemsCount;
        private int? _oldItemsCount;

        public CollectionChangeSpecification(NotifyCollectionChangedAction action)
        {
            this.Action = action;
        }

        public NotifyCollectionChangedAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public object[] OldItems
        {
            get { return _oldItems; }
            set { _oldItems = value; }
        }

        public object[] NewItems
        {
            get { return _newItems; }
            set { _newItems = value; }
        }

        public int? NewIndex
        {
            get { return _newIndex; }
            set { _newIndex = value; }
        }

        public int? OldIndex
        {
            get { return _oldIndex; }
            set { _oldIndex = value; }
        }

        public int? NewItemsCount
        {
            get { return _newItemsCount; }
            set { _newItemsCount = value; }
        }

        public int? OldItemsCount
        {
            get { return _oldItemsCount; }
            set { _oldItemsCount = value; }
        }

        public string Description
        {
            get 
            {
                string result = this.Action.ToString();
                if (this.GroupIndex != null)
                {
                    result += " on group " + this.GroupIndex.Value;
                }
                if (this.NewItems != null)
                {
                    result += ", new items " + PrintItems(this.NewItems);
                }
                if (this.OldItems != null)
                {
                    result += ", old items " + PrintItems(this.OldItems);
                }
                if (this.NewIndex != null)
                {
                    result += ", new index " + this.NewIndex;
                }
                if (this.OldIndex != null)
                {
                    result += ", original index " + this.OldIndex;
                }
                return result;
            }
        }

        public int? GroupIndex
        {
            get { return _groupIndex; }
            set { _groupIndex = value; }
        }

        public void CompareTo(NotifyCollectionChangedEventArgs argument)
        {
            if (argument.Action != this.Action)
            {
                Assert.Fail("Expected action {0} but got action {1} instead.".FormatWith(this.Action, argument.Action));
            }

            if (this.OldItems != null)
            {
                if (!CompareItems(argument.OldItems, this.OldItems))
                {
                    Assert.Fail("Actual old items '{0}' do not match expected items '{1}'", PrintItems(argument.OldItems), PrintItems(this.OldItems));
                }
            }
            if (this.NewItems != null)
            {
                if (!CompareItems(argument.NewItems, this.NewItems))
                {
                    Assert.Fail("Actual new items '{0}' do not match expected items '{1}'", PrintItems(argument.NewItems), PrintItems(this.NewItems));
                }
            }
            if (this.NewItemsCount != null)
            {
                Assert.AreEqual(this.NewItemsCount.Value, argument.NewItems.Count, "Actual new items '{0}' were not the correct number of items '{1}'", PrintItems(argument.NewItems), this.NewItemsCount.Value);
            }
            if (this.OldItemsCount != null)
            {
                Assert.AreEqual(this.OldItemsCount.Value, argument.OldItems.Count, "Actual old items '{0}' were not the correct number of items '{1}'", PrintItems(argument.OldItems), this.OldItemsCount.Value);
            }

            switch (this.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (this.NewIndex != null) Assert.AreEqual(this.NewIndex.Value, argument.NewStartingIndex, "NewStartingIndex for Add event was not as expected.");
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (this.NewIndex != null) Assert.AreEqual(this.NewIndex.Value, argument.NewStartingIndex, "NewStartingIndex for Move event was not as expected.");
                    if (this.OldIndex != null) Assert.AreEqual(this.OldIndex.Value, argument.OldStartingIndex, "OldStartingIndex for Move event was not as expected.");
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (this.OldIndex != null) Assert.AreEqual(this.OldIndex.Value, argument.OldStartingIndex, "OldStartingIndex for Remove event was not as expected.");
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (this.OldIndex != null) Assert.AreEqual(this.OldIndex.Value, argument.OldStartingIndex, "OldStartingIndex for Replace event was not as expected.");
                    break;
            }
        }

        private static bool CompareItems(IEnumerable actualItems, IEnumerable expectedItems)
        {
            var lhs = actualItems ?? new object[0];
            var rhs = expectedItems ?? new object[0];

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
                else if (!object.Equals(lhsEnumerator.Current, rhsEnumerator.Current))
                {
                    return false;
                }
            }
        }

        private static string PrintItems(IEnumerable items)
        {
            StringBuilder builder = new StringBuilder();
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
    }
}
