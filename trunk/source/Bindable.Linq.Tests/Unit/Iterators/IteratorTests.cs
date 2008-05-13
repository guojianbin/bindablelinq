using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Collections;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Dependencies.Definitions;
using Bindable.Linq.Iterators;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;
           
namespace Bindable.Linq.Tests.Unit.Iterators
{        
    [TestFixture]
    public class IteratorTests : TestFixture
    {
        [Test]
        public void IteratorWithDependenciesUnsubscribesFromAllSourceEventsWhenDisposed()
        {
            var sourceCollection = Given.Collection(Mike, Tom, Jack);

            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection);
            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.AcceptDependency(new ItemDependencyDefinition("Name"));

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            foreach (Contact c in contactIterator)
            {
            }

            Assert.IsTrue(Mike.HasPropertyChangedSubscribers);
            Assert.IsTrue(Tom.HasPropertyChangedSubscribers);
            Assert.IsTrue(Jack.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.Dispose();

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            Assert.IsFalse(sourceCollection.HasCollectionChangedSubscribers);
            Assert.IsFalse(sourceCollection.HasPropertyChangedSubscribers);
        }

        [Test]
        public void IteratorWithoutDependenciesUnsubscribesFromAllSourceEventsWhenDisposed()
        {
            var sourceCollection = Given.Collection(Mike, Tom, Jack);

            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection);
            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.GetEnumerator();

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.Dispose();

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            Assert.IsFalse(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsFalse(sourceCollection.HasCollectionChangedSubscribers);
        }

        [Test]
        public void IteratorUnsubscribesFromItemWhenRemoved()
        {
            var sourceCollection = Given.Collection(Mike, Tom, Jack);

            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection);
            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.AcceptDependency(new ItemDependencyDefinition("Name"));
            foreach (Contact c in contactIterator)
            {
            }

            Assert.IsTrue(Mike.HasPropertyChangedSubscribers);
            Assert.IsTrue(Tom.HasPropertyChangedSubscribers);
            Assert.IsTrue(Jack.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            sourceCollection.Remove(Tom);

            Assert.IsTrue(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsTrue(Jack.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.Dispose();

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            Assert.IsFalse(sourceCollection.HasPropertyChangedSubscribers);
            Assert.IsFalse(sourceCollection.HasCollectionChangedSubscribers);
        }

        [Test]
        public void IteratorWithDepenedenciesDoesNotEnumerateSourceUntilResultIsEnumerated()
        {
            SourceCollection sourceCollection = new SourceCollection(Given.Collection(Tom, Tim, Jack));
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            contactIterator.AcceptDependency(new ItemDependencyDefinition("Name"));
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            foreach (Contact c in contactIterator)
            {
            } 
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);
        }

        [Test]
        public void IListMethodsForceLoad()
        {
            ForcesLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.Contains(Tom));
            ForcesLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.GetEnumerator());
            ForcesLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.IndexOf(Tom));
            ForcesLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.Count);
            ForcesLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator[2]);
            ForcesLoad(Given.Collection(Tom, Sam, Sally), iterator => ((IList)iterator)[2]);
            ForcesLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.CopyTo(new Contact[3], 0));
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.Contains(new object()));
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.IndexOf(new object()));
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.CurrentCount);
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.IsReadOnly);
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.IsLoading);
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.IsSynchronized);
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.IsFixedSize);
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.SyncRoot);
            DoesNotForceLoad(Given.Collection(Tom, Sam, Sally), iterator => iterator.ToString());
        }

        [Test]
        public void WriteMethodsThrowExceptions()
        {
            var sourceCollection = Given.Collection(Mike, Tom, Jack);
            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection);
            
            IsNotSupported(() => contactIterator.Add(new object()));
            IsNotSupported(() => contactIterator.Clear());
            IsNotSupported(() => contactIterator.Insert(3, new object()));
            IsNotSupported(() => contactIterator.Remove(new object()));
            IsNotSupported(() => contactIterator.RemoveAt(2));
            IsNotSupported(() => contactIterator[0] = new Contact());
            IsNotSupported(() => ((IList)contactIterator)[0] = new Contact());
            Assert.IsTrue(contactIterator.IsReadOnly);
            Assert.IsTrue(contactIterator.IsSynchronized);
            Assert.IsFalse(contactIterator.IsFixedSize);
            Assert.AreNotEqual(contactIterator.SyncRoot, contactIterator.SyncRoot);
        }

        #region Test Helpers

        private static void ForcesLoad(BindableCollection<Contact> contacts, Action<SimpleIterator<Contact>> action)
        {
            SourceCollection sourceCollection = new SourceCollection(contacts);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            action(contactIterator);
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);
        }

        private static void ForcesLoad(BindableCollection<Contact> contacts, Func<SimpleIterator<Contact>, object> action)
        {
            SourceCollection sourceCollection = new SourceCollection(contacts);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            action(contactIterator);
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);
        }

        private static void DoesNotForceLoad(BindableCollection<Contact> contacts, Action<SimpleIterator<Contact>> action)
        {
            SourceCollection sourceCollection = new SourceCollection(contacts);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            action(contactIterator);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);
        }

        private static void DoesNotForceLoad(BindableCollection<Contact> contacts, Func<SimpleIterator<Contact>, object> action)
        {
            SourceCollection sourceCollection = new SourceCollection(contacts);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            SimpleIterator<Contact> contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            action(contactIterator);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);
        }

        private static void IsNotSupported(Action action)
        {
            bool threw = false;
            try
            {
                action();
            }
            catch (NotSupportedException)
            {
                threw = true;
            }
            if (!threw)
            {
                Assert.Fail("Exception was not thrown.");
            }
        }

        #endregion

        #region Test Collection

        private class SourceCollection : IEnumerable<Contact>
        {
            private BindableCollection<Contact> _inner;
            private int _getEnumeratorCalls;

            public SourceCollection(BindableCollection<Contact> inner)
            {
                _inner = inner;
            }

            public int GetEnumeratorCalls
            {
                get
                {
                    return _getEnumeratorCalls;
                }
            }

            public IEnumerator<Contact> GetEnumerator()
            {
                _getEnumeratorCalls++;
                return _inner.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #endregion

        #region Test Iterator

        private class SimpleIterator<TElement> : Iterator<TElement, TElement> where TElement : class
        {
            public SimpleIterator(IBindableCollection<TElement> sourceCollection)
                : base(sourceCollection)
            {
            }

            protected override void LoadSourceCollection()
            {
                this.IteratorLock.MustNotBeHeld();
                this.ReactToAddRange(0, this.SourceCollection);
            }

            protected override void ReactToAddRange(int sourceStartingIndex, IEnumerable<TElement> addedItems)
            {
                this.IteratorLock.MustNotBeHeld();
                this.ResultCollection.AddOrInsertRange(sourceStartingIndex, addedItems);
            }

            protected override void ReactToMoveRange(int sourceStartingIndex, IEnumerable<TElement> movedItems)
            {
                this.IteratorLock.MustNotBeHeld();
            }

            protected override void ReactToRemoveRange(IEnumerable<TElement> removedItems)
            {
                this.IteratorLock.MustNotBeHeld();
            }

            protected override void ReactToReplaceRange(IEnumerable<TElement> oldItems, IEnumerable<TElement> newItems)
            {
                this.IteratorLock.MustNotBeHeld();
            }

            protected override void ReactToItemPropertyChanged(TElement item, string propertyName)
            {
                this.IteratorLock.MustNotBeHeld();
            }
        }

        #endregion
    }
}
