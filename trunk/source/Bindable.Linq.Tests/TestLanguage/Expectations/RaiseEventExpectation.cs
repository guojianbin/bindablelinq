using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using NUnit.Framework;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    internal sealed class RaiseEventExpectation : IExpectation
    {
        public RaiseEventExpectation()
        {

        }

        public NotifyCollectionChangedAction? Action { get; set; }
        public object[] OldItems { get; set; }
        public object[] NewItems { get; set; }
        public int? NewIndex { get; set; }
        public int? OldIndex { get; set; }
        public int? NewItemsCount { get; set; }
        public int? OldItemsCount { get; set; }
        public int? GroupIndex { get; set; }

        public override string ToString()
        {
            var result = this.Action.ToString();
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

        public void Validate(IScenario scenario)
        {
            var lastEvent = scenario.EventMonitor.DequeueNextEvent();
            if (lastEvent != null)
            {
                this.CompareTo(lastEvent.Arguments);
            }
            else
            {
                this.CompareTo(null);
            }
        }

        private static bool CompareItems(IEnumerable actualItems, IEnumerable expectedItems)
        {
            var lhs = actualItems ?? new object[0];
            var rhs = expectedItems ?? new object[0];

            var lhsEnumerator = lhs.GetEnumerator();
            var rhsEnumerator = rhs.GetEnumerator();

            while (true)
            {
                var lhsHasNext = lhsEnumerator.MoveNext();
                var rhsHasNext = rhsEnumerator.MoveNext();

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
                foreach (var item in items)
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
            if (this.Action == null && argument != null)
            {
                Assert.Fail("No events were expected at this point, but instead a {0} event was raised with old items '{1}' and new items '{2}'".FormatWith(argument.Action, PrintItems(argument.OldItems), PrintItems(argument.NewItems)));
            }
            if (this.Action != null && argument == null)
            {
                Assert.Fail("An {0} event was expected at this point but was not raised. Expected new items were '{0}', expected old items were '{1}'".FormatWith(this.Action.Value, PrintItems(this.OldItems), PrintItems(this.NewItems)));
            }
            if (this.Action == null && argument == null)
            {
                return;
            }

            if (argument.Action != Action.Value)
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
