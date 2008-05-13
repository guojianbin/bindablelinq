using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;
using Bindable.Linq.Tests.TestHelpers;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// Contains unit tests for the <see cref="T:PollIterator`1"/> class.
    /// </summary>
    [TestFixture]
    public class PollBehaviour : TestFixture
    {
        #region Non-bindable Collection
        
        internal class NonBindableCollection : IEnumerable<Contact>
        {
            public List<Contact> Items = new List<Contact>();
            public int GetEnumeratorCalls = 0;

            public NonBindableCollection(params Contact[] contacts)
            {
                this.Items.AddRange(contacts);
            }

            public IEnumerator<Contact> GetEnumerator()
            {
                GetEnumeratorCalls++;
                return Items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(Contact item)
            {
                this.Items.Add(item);
            }
        }

        #endregion

        /// <summary>
        /// Tests the poll Iterator.
        /// </summary>
        [Test]
        public void PollIteratorPollsForChanges()
        {
            NonBindableCollection nonBindableCollection = new NonBindableCollection(Tom, Mike);

            Given.ExistingCollection(nonBindableCollection)
                .WithSyncLinqQuery(collection => collection.AsBindable().Polling(TimeSpan.FromMilliseconds(100)))
                .AndLinqEquivalent(collection => collection)
                .WhenLoaded().ExpectNoEvents().AndCountOf(2)
                .ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 1)
                .WaitFor(250)
                .ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 3)
                .AndExpectFinalCountOf(2);
        }

        /// <summary>
        /// Tests that the PollIterator picks up newly added items.
        /// </summary>
        [Test]
        public void PollIteratorDetectsAdditions()
        {
            NonBindableCollection nonBindableCollection = new NonBindableCollection(Tom, Mike);
            
            Given.ExistingCollection(nonBindableCollection)
                .WithSyncLinqQuery(collection => collection.AsBindable().Polling(TimeSpan.FromMilliseconds(100)))
                .AndLinqEquivalent(collection => collection)
                .WhenLoaded().ExpectNoEvents().AndCountOf(2)
                .ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 1)
                .ThenExecute(delegate { nonBindableCollection.Items.Add(Mick); })
                .WaitFor(250)
                .ExpectEvent(Add.WithNewItems(Mick).WithNewIndex(2))
                .ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 3)
                .AndExpectFinalCountOf(3);
        }

        /// <summary>
        /// Tests that the PollIterator picks up removed items.
        /// </summary>
        [Test]
        public void PollIteratorDetectsRemovals()
        {
            NonBindableCollection nonBindableCollection = new NonBindableCollection(Tom, Mike);

            Given.ExistingCollection(nonBindableCollection)
                .WithSyncLinqQuery(collection => collection.AsBindable().Polling(TimeSpan.FromMilliseconds(100)))
                .AndLinqEquivalent(collection => collection)
                .WhenLoaded().ExpectNoEvents().AndCountOf(2)
                .ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 1)
                .ThenExecute(delegate { nonBindableCollection.Items.Remove(Tom); })
                .WaitFor(250)
                .ExpectEvent(Remove.WithOldItems(Tom).WithOldIndex(0))
                .ExpectEqual(nonBindableCollection.GetEnumeratorCalls, 3)
                .AndExpectFinalCountOf(1);
        }
    }
}
