using System;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Threading;
    using Collections;
    using NUnit.Framework;
    using TestHelpers;
    using TestObjectModel;

    /// <summary>
    /// Contains unit tests for the <see cref="T:AsynchronousIterator`1"/> class.
    /// </summary>
    [TestFixture]
    public class AsynchronousBehaviour : TestFixture
    {
        internal class SlowCollection : IEnumerable<Contact>, INotifyCollectionChanged
        {
            private readonly BindableCollection<Contact> _items = new BindableCollection<Contact>();
            private readonly object _lock = new object();
            private bool _paused = true;

            public SlowCollection(params Contact[] contacts)
            {
                _items.AddRange(contacts);
            }

            #region IEnumerable<Contact> Members
            public IEnumerator<Contact> GetEnumerator()
            {
                Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Getting enumerator for items in SlowCollection");
                while (_paused)
                {
                    Thread.Sleep(10);
                }

                lock (_lock)
                {
                    return _items.GetEnumerator();
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion

            #region INotifyCollectionChanged Members
            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add { _items.CollectionChanged += value; }
                remove { _items.CollectionChanged -= value; }
            }
            #endregion

            public void Play()
            {
                _paused = false;
            }

            public void Add(Contact contact)
            {
                _items.Add(contact);
            }
        }

        [Test]
        public void AsynchronousIteratorInitializeIsEmptyAndItemsAreStreamed()
        {
            var slowCollection = new SlowCollection(Tom, Tim, Jarryd);

            Given.ExistingCollection(slowCollection).WithSyncLinqQuery(collection => collection.AsBindable().Asynchronous(new TestDispatcher())).AndLinqEquivalent(collection => collection).ExpectingTheyAre(CompatibilityExpectation.FullyCompatibleExceptOrdering).WhenLoaded().ExpectNoEvents().AndCountOf(0).ThenExecute(delegate { slowCollection.Play(); }).WaitFor(250).ExpectEvent(Add.WithNewItems(Tom).WithNewIndex(0)).ExpectEvent(Add.WithNewItems(Tim).WithNewIndex(1)).ExpectEvent(Add.WithNewItems(Jarryd).WithNewIndex(2)).AndExpectFinalCountOf(3);
        }
    }
}