namespace Bindable.Linq.Tests.Unit.Eventing
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using NUnit.Framework;
    using TestHelpers;
    using Transactions;

    [TestFixture]
    public class CollectionChangetransactionTests : TestFixture
    {
        private class MockPublisher
        {
            private readonly Queue<NotifyCollectionChangedEventArgs> _arguments;

            public MockPublisher()
            {
                _arguments = new Queue<NotifyCollectionChangedEventArgs>();
            }

            public void CommitCallback(TransactionLog transactionLog)
            {
                foreach (var eventToRaise in transactionLog.Events)
                {
                    _arguments.Enqueue(eventToRaise);
                }
            }

            public void Expect(CollectionChangeSpecification specification)
            {
                if (_arguments.Count > 0)
                {
                    var lastEvent = _arguments.Dequeue();
                    if (lastEvent != null)
                    {
                        specification.CompareTo(lastEvent);
                    }
                }
                else
                {
                    Assert.Fail("Event was not raised.");
                }
            }

            public void ExpectNoEvents()
            {
                Assert.AreEqual(_arguments.Count, 0);
            }
        }

        [Test]
        public void AdjacentAddsAreCombined()
        {
            var publisher = new MockPublisher();
            using (var transaction = new Transaction(publisher.CommitCallback))
            {
                transaction.LogAddEvent(Mike, 1);
                transaction.LogAddEvent(Sam, 32);
                transaction.LogAddEvent(Tim, 2);
                publisher.ExpectNoEvents();
            }
            publisher.Expect(Add.WithNewIndex(1).WithNewItems(Mike, Tim));
            publisher.Expect(Add.WithNewIndex(32).WithNewItems(Sam));
        }

        [Test]
        public void MatchingAddsAreNotCombined()
        {
            var publisher = new MockPublisher();
            using (var transaction = new Transaction(publisher.CommitCallback))
            {
                transaction.LogAddEvent(Mike, 1);
                transaction.LogAddEvent(Sam, 32);
                transaction.LogAddEvent(Tim, 1);
                publisher.ExpectNoEvents();
            }
            publisher.Expect(Add.WithNewIndex(1).WithNewItems(Mike));
            publisher.Expect(Add.WithNewIndex(32).WithNewItems(Sam));
            publisher.Expect(Add.WithNewIndex(1).WithNewItems(Tim));
        }

        [Test]
        public void NonAdjacentAddsAreNotCombined()
        {
            var publisher = new MockPublisher();
            using (var transaction = new Transaction(publisher.CommitCallback))
            {
                transaction.LogAddEvent(Mike, 1);
                transaction.LogAddEvent(Sam, 32);
                transaction.LogAddEvent(Tim, 3);
                publisher.ExpectNoEvents();
            }
            publisher.Expect(Add.WithNewIndex(1).WithNewItems(Mike));
            publisher.Expect(Add.WithNewIndex(32).WithNewItems(Sam));
            publisher.Expect(Add.WithNewIndex(3).WithNewItems(Tim));
        }
    }
}