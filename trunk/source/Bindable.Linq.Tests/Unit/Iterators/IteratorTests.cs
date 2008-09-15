using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Bindable.Linq.Collections;
using Bindable.Linq.Dependencies.Definitions;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Iterators;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace Bindable.Linq.Tests.Unit.Iterators
{
    [TestFixture]
    public class IteratorTests : TestFixture
    {
        #region Test Helpers
        private static void ForcesLoad(ObservableCollection<Contact> contacts, Action<SimpleIterator<Contact>> action)
        {
            var sourceCollection = new SourceCollection(contacts);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            var contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            action(contactIterator);
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);
        }

        private static void ForcesLoad(ObservableCollection<Contact> contacts, Func<SimpleIterator<Contact>, object> action)
        {
            var sourceCollection = new SourceCollection(contacts);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            var contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            action(contactIterator);
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);
        }

        private static void DoesNotForceLoad(ObservableCollection<Contact> contacts, Action<SimpleIterator<Contact>> action)
        {
            var sourceCollection = new SourceCollection(contacts);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            var contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            action(contactIterator);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);
        }

        private static void DoesNotForceLoad(ObservableCollection<Contact> contacts, Func<SimpleIterator<Contact>, object> action)
        {
            var sourceCollection = new SourceCollection(contacts);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            var contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            action(contactIterator);
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);
        }

        private static void IsNotSupported(Action action)
        {
            var threw = false;
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

        private class SourceCollection : IEnumerable<Contact>
        {
            private readonly ObservableCollection<Contact> _inner;
            private int _getEnumeratorCalls;

            public SourceCollection(ObservableCollection<Contact> inner)
            {
                _inner = inner;
            }

            public int GetEnumeratorCalls
            {
                get { return _getEnumeratorCalls; }
            }

            #region IEnumerable<Contact> Members
            public IEnumerator<Contact> GetEnumerator()
            {
                _getEnumeratorCalls++;
                return _inner.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion
        }

        private class SimpleIterator<TElement> : Iterator<TElement, TElement>
            where TElement : class
        {
            public SimpleIterator(IEnumerable<TElement> sourceCollection)
                : base(sourceCollection.AsBindable(), new TestDispatcher()) { }

            private void EnsureLockIsNotHeld()
            {
                var isHeld = true;
                var are = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(delegate
                {
                    if (Monitor.TryEnter(InstanceLock, TimeSpan.FromMilliseconds(10)))
                    {
                        Monitor.Exit(InstanceLock);
                        isHeld = false;
                    }
                    are.Set();
                });
                are.WaitOne();
                if (isHeld)
                {
                    Assert.Fail("Lock is held");
                }
            }

            protected override void EvaluateSourceCollection()
            {
                EnsureLockIsNotHeld();
                foreach (var o in SourceCollection)
                    ReactToAdd(-1, o);
            }

            protected override void ReactToAdd(int insertionIndex, TElement addedItem)
            {
                EnsureLockIsNotHeld();
                ResultCollection.Insert(insertionIndex, addedItem);
            }

            protected override void ReactToMove(int oldIndex, int newIndex, TElement movedItem)
            {
                EnsureLockIsNotHeld();
            }

            protected override void ReactToRemove(int oldIndex, TElement removedItem)
            {
                EnsureLockIsNotHeld();
            }

            protected override void ReactToReplace(int oldIndex, TElement oldItem, TElement newItem)
            {
                EnsureLockIsNotHeld();
            }

            protected override void ReactToItemPropertyChanged(TElement item, string propertyName)
            {
                EnsureLockIsNotHeld();
            }
        }
        #endregion

        [Test]
        public void IteratorIListMethodsForceLoad()
        {
            ForcesLoad(With.Inputs(Tom, Sam, Sally), iterator => iterator.Contains(Tom));
            ForcesLoad(With.Inputs(Tom, Sam, Sally), iterator => iterator.GetEnumerator());
            ForcesLoad(With.Inputs(Tom, Sam, Sally), iterator => iterator.Count);
            // TODO: ForcesLoad(With.Inputs(Tom, Sam, Sally), iterator => iterator[2]);
            ForcesLoad(With.Inputs(Tom, Sam, Sally), iterator => ((IList)iterator)[2]);
            DoesNotForceLoad(With.Inputs(Tom, Sam, Sally), iterator => iterator.ToString());
        }

        [Test]
        public void IteratorUnsubscribesFromItemWhenRemoved()
        {
            var sourceCollection = With.Inputs(Mike, Tom, Jack);

            var contactIterator = new SimpleIterator<Contact>(sourceCollection);
            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.AcceptDependency(new ItemDependencyDefinition("Name"));
            foreach (var c in contactIterator) { }

            Assert.IsTrue(Mike.HasPropertyChangedSubscribers);
            Assert.IsTrue(Tom.HasPropertyChangedSubscribers);
            Assert.IsTrue(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            sourceCollection.Remove(Tom);

            Assert.IsTrue(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsTrue(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.Dispose();

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsFalse(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsFalse(sourceCollection.HasCollectionChangedSubscribers);
        }

        [Test]
        public void IteratorWithDependenciesUnsubscribesFromAllSourceEventsWhenDisposed()
        {
            var sourceCollection = With.Inputs(Mike, Tom, Jack);

            var contactIterator = new SimpleIterator<Contact>(sourceCollection);
            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.AcceptDependency(new ItemDependencyDefinition("Name"));

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            foreach (var c in contactIterator) { }

            Assert.IsTrue(Mike.HasPropertyChangedSubscribers);
            Assert.IsTrue(Tom.HasPropertyChangedSubscribers);
            Assert.IsTrue(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.Dispose();

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsFalse(sourceCollection.HasCollectionChangedSubscribers);
            // TODO: Assert.IsFalse(sourceCollection.HasPropertyChangedSubscribers);
        }

        [Test]
        public void IteratorWithDepenedenciesDoesNotEnumerateSourceUntilResultIsEnumerated()
        {
            var sourceCollection = new SourceCollection(With.Inputs(Tom, Tim, Jack));
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            var contactIterator = new SimpleIterator<Contact>(sourceCollection.AsBindable());
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            contactIterator.AcceptDependency(new ItemDependencyDefinition("Name"));
            Assert.AreEqual(0, sourceCollection.GetEnumeratorCalls);

            foreach (var c in contactIterator) { }
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);

            contactIterator.Dispose();
            Assert.AreEqual(1, sourceCollection.GetEnumeratorCalls);
        }

        [Test]
        public void IteratorWithoutDependenciesUnsubscribesFromAllSourceEventsWhenDisposed()
        {
            var sourceCollection = With.Inputs(Mike, Tom, Jack);

            var contactIterator = new SimpleIterator<Contact>(sourceCollection);
            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.GetEnumerator();

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsTrue(sourceCollection.HasCollectionChangedSubscribers);

            contactIterator.Dispose();

            Assert.IsFalse(Mike.HasPropertyChangedSubscribers);
            Assert.IsFalse(Tom.HasPropertyChangedSubscribers);
            Assert.IsFalse(Jack.HasPropertyChangedSubscribers);
            // TODO: Assert.IsFalse(sourceCollection.HasPropertyChangedSubscribers);
            // TODO: Assert.IsFalse(sourceCollection.HasCollectionChangedSubscribers);
        }    
    }
}