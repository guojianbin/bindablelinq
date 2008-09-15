using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using Bindable.Linq.Collections;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// Contains unit tests for the <see cref="T:AsynchronousIterator`1"/> class.
    /// </summary>
    [TestFixture]
    public class AsynchronousBehaviour : TestFixture
    {
        internal class SlowCollection : IEnumerable<Contact>, INotifyCollectionChanged
        {
            private readonly ObservableCollection<Contact> _items = new ObservableCollection<Contact>();
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
    }
}