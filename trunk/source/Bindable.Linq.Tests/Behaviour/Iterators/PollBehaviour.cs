using System;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using TestHelpers;
    using TestObjectModel;

    /// <summary>
    /// Contains unit tests for the <see cref="T:PollIterator`1"/> class.
    /// </summary>
    [TestFixture]
    public class PollBehaviour : TestFixture
    {
        internal class NonBindableCollection : IEnumerable<Contact>
        {
            public int GetEnumeratorCalls;
            public List<Contact> Items = new List<Contact>();

            public NonBindableCollection(params Contact[] contacts)
            {
                Items.AddRange(contacts);
            }

            #region IEnumerable<Contact> Members
            public IEnumerator<Contact> GetEnumerator()
            {
                GetEnumeratorCalls++;
                return Items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion

            public void Add(Contact item)
            {
                Items.Add(item);
            }
        }

        /// <summary>
        /// Tests that the PollIterator picks up newly added items.
        /// </summary>
        [Test]
        public void PollIteratorDetectsAdditions()
        {
            var nonBindableCollection = new NonBindableCollection(Tom, Mike);

            Given.ExistingCollection(nonBindableCollection).WithSyncLinqQuery(collection => collection.AsBindable().Polling(new TestDispatcher(), TimeSpan.FromMilliseconds(100))).AndLinqEquivalent(collection => collection).WhenLoaded().ExpectNoEvents().AndCountOf(2).ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 1).ThenExecute(delegate { nonBindableCollection.Items.Add(Mick); }).WaitFor(250).ExpectEvent(Add.WithNewItems(Mick).WithNewIndex(2)).ExpectGreaterOrEqual(nonBindableCollection.GetEnumeratorCalls, 3).AndExpectFinalCountOf(3);
        }

        /// <summary>
        /// Tests that the PollIterator picks up removed items.
        /// </summary>
        [Test]
        public void PollIteratorDetectsRemovals()
        {
            var nonBindableCollection = new NonBindableCollection(Tom, Mike);

            Given.ExistingCollection(nonBindableCollection).WithSyncLinqQuery(collection => collection.AsBindable().Polling(new TestDispatcher(), TimeSpan.FromMilliseconds(100))).AndLinqEquivalent(collection => collection).WhenLoaded().ExpectNoEvents().AndCountOf(2).ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 1).ThenExecute(delegate { nonBindableCollection.Items.Remove(Tom); }).WaitFor(250).ExpectEvent(Remove.WithOldItems(Tom).WithOldIndex(0)).ExpectGreaterOrEqual(nonBindableCollection.GetEnumeratorCalls, 3).AndExpectFinalCountOf(1);
        }

        /// <summary>
        /// Tests the poll Iterator.
        /// </summary>
        [Test]
        public void PollIteratorPollsForChanges()
        {
            var nonBindableCollection = new NonBindableCollection(Tom, Mike);

            Given.ExistingCollection(nonBindableCollection).WithSyncLinqQuery(collection => collection.AsBindable().Polling(new TestDispatcher(), TimeSpan.FromMilliseconds(100))).AndLinqEquivalent(collection => collection).WhenLoaded().ExpectNoEvents().AndCountOf(2).ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 1).WaitFor(250).ExpectGreaterOrEqual(nonBindableCollection.GetEnumeratorCalls, 3).AndExpectFinalCountOf(2);
        }
    }
}