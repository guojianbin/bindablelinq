using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Bindable.Linq.Collections;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// Contains unit tests for the <see cref="T:AsynchronousIterator`1"/> class.
    /// </summary>
    [TestFixture]
    public class AsynchronousBehaviour : TestFixture
    {
        #region Slow testing data source

        internal class SlowCollection : IEnumerable<Contact>, INotifyCollectionChanged
        {
            private readonly object _lock = new object();
            private BindableCollection<Contact> _items = new BindableCollection<Contact>();
            private bool _paused = true;

            public SlowCollection(params Contact[] contacts)
            {
                this._items.AddRange(contacts);
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add
                {
                    _items.CollectionChanged += value;
                }
                remove
                {
                    _items.CollectionChanged -= value;
                }
            }

            public IEnumerator<Contact> GetEnumerator()
            {
                Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " Getting enumerator for items in SlowCollection");
                while (_paused)
                    Thread.Sleep(10);

                lock (_lock)
                {
                    return this._items.GetEnumerator();
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Play()
            {
                _paused = false;
            }

            public void Add(Contact contact)
            {
                _items.Add(contact);
            }
        }

        #endregion

        [Test]
        public void AsynchronousIteratorInitializeIsEmptyAndItemsAreStreamed()
        {
            SlowCollection slowCollection = new SlowCollection(Tom, Tim, Jarryd);

            Given.ExistingCollection(slowCollection)
                 .WithSyncLinqQuery(collection => collection.AsBindable().Asynchronous())
                 .AndLinqEquivalent(collection => collection)
                 .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                 .WhenLoaded().ExpectNoEvents().AndCountOf(0)
                 .ThenExecute(delegate { slowCollection.Play(); })
                 .WaitFor(200)
                 .ExpectEvent(Add.WithNewItems(Tom).WithNewIndex(0))
                 .ExpectEvent(Add.WithNewItems(Tim).WithNewIndex(1))
                 .ExpectEvent(Add.WithNewItems(Jarryd).WithNewIndex(2))
                 .AndExpectFinalCountOf(3);
        }
    }
}
