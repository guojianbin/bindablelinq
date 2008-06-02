using System;

namespace Bindable.Linq.Tests.TestHelpers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Threading;
    using Collections;
    using NUnit.Framework;

    /// <summary>
    /// Contains a number of extension methods used for writing unit tests against the various iterators.
    /// </summary>
    internal static class IteratorTestExtensions
    {
        /// <summary>
        /// Sets the Bindable LINQ version of the query to test. 
        /// </summary>
        public static IteratorTestState<TInput, TResult> WithSyncLinqQuery<TInput, TResult>(this IEnumerable<TInput> inputs, Func<IEnumerable<TInput>, IBindableCollection<TResult>> queryCreator)
        {
            var testState = new IteratorTestState<TInput, TResult>();
            testState.Inputs = inputs;
            testState.SyncLinqQuery = queryCreator(testState.Inputs);
            return testState;
        }

        /// <summary>
        /// Sets the standard LINQ version of the query to test. The results of the Bindable LINQ query 
        /// will be compared against this query.
        /// </summary>
        public static IteratorTestState<TInput, TResult> AndLinqEquivalent<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, Func<IEnumerable<TInput>, IEnumerable> queryCreator)
        {
            testState.LinqEquivalent = queryCreator(testState.Inputs);
            return testState;
        }

        /// <summary>
        /// Inidicates that the query shouldn't be initialized (GetEnumerator will not be called) and 
        /// checks that the current count is still 0.
        /// </summary>
        public static IteratorTestState<TInput, TResult> WithoutInitializing<TInput, TResult>(this IteratorTestState<TInput, TResult> testState)
        {
            Trace.WriteLine("Executing step: WithoutInitializing");
            Assert.AreEqual(0, GetCurrentCount(testState), "Since the query result should not have been initialized yet, the CurrentCount should be 0");
            Assert.IsNull(testState.Events.DequeueNextEvent(), "Since the query result should not have been initialized yet, no events should have been raised");
            return testState;
        }

        /// <summary>
        /// Initializes the test by calling GetEnumerator on the Bindable LINQ query, and verifying the results 
        /// against the LINQ version.
        /// </summary>
        /// <param name="testState">The query tester.</param>
        /// <returns></returns>
        public static IteratorTestState<TInput, TResult> WhenLoaded<TInput, TResult>(this IteratorTestState<TInput, TResult> testState)
        {
            Trace.WriteLine("Executing step: WhenLoaded");
            Assert.AreEqual(0, GetCurrentCount(testState), "The query is yet to be initialized, and so the CurrentCount ought to be 0");
            Assert.IsNull(testState.Events.DequeueNextEvent(), "The query is yet to be initialized, and so no events should have been raised");
            testState.Results.GetEnumerator();
            Assert.IsNull(testState.Events.DequeueNextEvent(), "The query was initialized, but this should not have triggered any events");
            return testState;
        }

        /// <summary>
        /// Tells the test that when comparing the LINQ query to the Bindable LINQ query, the ordering of the 
        /// items in each collection doesn't matter - so long as they have the same items.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ExpectingTheyAre<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, CompatibilityExpectation expectation)
        {
            Trace.WriteLine("Executing step: ExpectingTheyAre");
            testState.CompatibilityExpectation = expectation;
            return testState;
        }

        /// <summary>
        /// Adds any number of new items to the inputs.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenAdd<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, params TInput[] inputs)
        {
            Trace.WriteLine("Executing step: ThenAdd");
            var testInputs = testState.Inputs as BindableCollection<TInput>;
            if (testInputs != null)
            {
                if (inputs.Length == 0)
                {
                    testInputs.Add(inputs[0]);
                }
                else
                {
                    testInputs.AddRange(inputs);
                }
            }
            return testState;
        }

        /// <summary>
        /// Applies any code change to the inputs.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenChange<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, Action<BindableCollection<TInput>> changeAction)
        {
            Trace.WriteLine("Executing step: ThenChange");
            var testInputs = testState.Inputs as BindableCollection<TInput>;
            if (testInputs != null)
            {
                changeAction(testInputs);
            }

            return testState;
        }

        /// <summary>
        /// Removes any number of items from the inputs.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenRemove<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, params TInput[] inputs)
        {
            Trace.WriteLine("Executing step: ThenRemove");
            var testInputs = testState.Inputs as BindableCollection<TInput>;
            if (testInputs != null)
            {
                if (inputs.Length == 1)
                {
                    testInputs.Remove(inputs[0]);
                }
                else
                {
                    testInputs.RemoveRange(inputs);
                }
            }
            return testState;
        }

        /// <summary>
        /// Replaces an item in the inputs with a new item.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenReplace<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, TInput oldItem, TInput newItem)
        {
            Trace.WriteLine("Executing step: ThenReplace");
            var testInputs = testState.Inputs as BindableCollection<TInput>;
            if (testInputs != null)
            {
                testInputs.Replace(oldItem, newItem);
            }
            return testState;
        }

        /// <summary>
        /// Replaces any number of input items with any number of replacements.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenReplace<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, TInput[] oldNames, TInput[] newNames)
        {
            Trace.WriteLine("Executing step: ThenReplace");
            var testInputs = testState.Inputs as BindableCollection<TInput>;
            if (testInputs != null)
            {
                testInputs.ReplaceRange(oldNames, newNames);
            }
            return testState;
        }

        /// <summary>
        /// Moves a single item.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenMove<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, int newIndex, TInput input)
        {
            Trace.WriteLine("Executing step: ThenMove");
            var testInputs = testState.Inputs as BindableCollection<TInput>;
            if (testInputs != null)
            {
                testInputs.Move(newIndex, input);
            }
            return testState;
        }

        /// <summary>
        /// Moves a single item.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenMove<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, int newIndex, params TInput[] inputs)
        {
            Trace.WriteLine("Executing step: ThenMove");
            var testInputs = testState.Inputs as BindableCollection<TInput>;
            if (testInputs != null)
            {
                testInputs.MoveRange(newIndex, inputs);
            }
            return testState;
        }

        /// <summary>
        /// Initializes the query by iterating it.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenInitialize<TInput, TResult>(this IteratorTestState<TInput, TResult> testState)
        {
            Trace.WriteLine("Executing step: ThenInitialize");
            return WhenLoaded(testState);
        }

        /// <summary>
        /// Refreshes the source collection.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ThenRefresh<TInput, TResult>(this IteratorTestState<TInput, TResult> testState)
        {
            Trace.WriteLine("Executing step: ThenRefresh");
            ((IBindableQuery) testState.Results).Refresh();
            return testState;
        }

        public static IteratorTestState<TInput, TResult> ThenExecute<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, Action action)
        {
            Trace.WriteLine("Executing step: ThenExecute");
            action();
            return testState;
        }

        public static IteratorTestState<TInput, TResult> WaitFor<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, int milliseconds)
        {
            Trace.WriteLine("Executing step: WaitFor");
            Thread.Sleep(milliseconds);
            return testState;
        }

        public static IteratorTestState<TInput, TResult> ExpectThat<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, Func<bool> callback)
        {
            Trace.WriteLine("Executing step: ExpectThat");
            bool result = callback();
            Assert.IsTrue(result);
            return testState;
        }

        public static IteratorTestState<TInput, TResult> ExpectEqual<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, object actual, object expected)
        {
            Trace.WriteLine("Executing step: ExpectAreEqual");
            Assert.AreEqual(expected, actual);
            return testState;
        }

        public static IteratorTestState<TInput, TResult> ExpectGreaterOrEqual<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, int actual, int expected)
        {
            Trace.WriteLine("Executing step: ExpectGreaterOrEqual");
            Assert.GreaterOrEqual(actual, expected);
            return testState;
        }

        /// <summary>
        /// Asserts that no events were raised by the last action.
        /// </summary>
        public static IteratorTestState<TInput, TResult> ExpectNoEvents<TInput, TResult>(this IteratorTestState<TInput, TResult> testState)
        {
            Trace.WriteLine("Executing step: ExpectNoEvents");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            NotifyCollectionChangedEventArgs e = testState.Events.DequeueNextEvent();
            if (e != null)
            {
                Assert.Fail("The query was not supposed to have raised any new events at this point, but raised: {0}", e.Action);
            }
            return testState;
        }

        public static IteratorTestState<TInput, TResult> ExpectEvent<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, CollectionChangeSpecification specification)
        {
            Trace.WriteLine("Executing step: Expect Event: " + specification.Description);
            NotifyCollectionChangedEventArgs lastEvent = GetLastEvent(specification.GroupIndex, testState);
            Assert.IsNotNull(lastEvent, "An event was expected at this point: {0}", specification.Description);
            specification.CompareTo(lastEvent);
            return testState;
        }

        public static IteratorTestState<TInput, TResult> ExpectNoEventsOnGroup<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, int childIndex)
        {
            Trace.WriteLine("Executing step: ExpectNoEventsOnGroup: " + childIndex);
            NotifyCollectionChangedEventArgs lastEvent = GetLastEvent(childIndex, testState);
            Assert.IsNull(lastEvent, "No events were expected at this point");
            return testState;
        }

        /// <summary>
        /// Compares the two queries and ensures there are no additional events that weren't expected.
        /// </summary>
        /// <param name="testState">The query tester.</param>
        public static IteratorTestState<TInput, TResult> AndCountOf<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, int count)
        {
            Trace.WriteLine("Executing step: AndCountOf");
            Assert.AreEqual(count, CountAll(testState.Results), "The number of items in the query was incorrect.");
            return testState;
        }

        /// <summary>
        /// Compares the two queries and ensures there are no additional events that weren't expected.
        /// </summary>
        /// <param name="testState">The query tester.</param>
        public static void AndExpectFinalCountOf<TInput, TResult>(this IteratorTestState<TInput, TResult> testState, int count)
        {
            Trace.WriteLine("Executing step: AndExpectFinalCountOf");
            CompareResults(testState);
            Assert.AreEqual(count, CountAll(testState.Results), "The number of items in the query was incorrect.");
            Assert.IsNull(testState.Events.DequeueNextEvent(), "Since the test is finished, there should be no unexpected events raised at this point");

            //testState.SyncLinqQuery.Dispose(); 
            testState.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.IsFalse(testState.IsQueryAlive(), "The Bindable LINQ query is still alive even though all references to it should have been disposed. Check for any event subscriptions still active.");
        }

        private static void CompareResults<TInput, TResult>(IteratorTestState<TInput, TResult> testState)
        {
            CompatibilityValidator.CompareWithLinq(testState.CompatibilityExpectation, testState.Results, testState.LinqEquivalent);
        }

        private static int CountAll(IEnumerable e)
        {
            int i = 0;
            foreach (object o in e)
            {
                i++;
            }
            return i;
        }

        private static int GetCurrentCount<TInput, TResult>(IteratorTestState<TInput, TResult> testState)
        {
            int result = 0;
            if (testState.Results is IBindableQuery)
            {
                result = ((IBindableQuery) testState.Results).CurrentCount;
            }
            return result;
        }

        private static NotifyCollectionChangedEventArgs GetLastEvent<TInput, TResult>(int? group, IteratorTestState<TInput, TResult> testState)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            object publisher = testState.SyncLinqQuery;
            if (group != null)
            {
                int i = 0;
                foreach (object p in testState.SyncLinqQuery)
                {
                    if (i == group.Value)
                    {
                        publisher = p;
                    }
                    i++;
                }
            }

            if (publisher != null)
            {
                return testState.Events.DequeueNextEvent((INotifyCollectionChanged) publisher);
            }
            return null;
        }
    }
}