using System.Collections.Generic;
using System.Collections.Specialized;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.TestLanguage.Expectations;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Unit.Eventing
{
    [TestFixture]
    public class TransactionTests : TestFixture
    {
        #region Test Helpers
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

            public void Expect(RaiseEventExpectation specification)
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
        #endregion

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
            publisher.Expect(Add.AtNew(1).WithNew(Mike, Tim));
            publisher.Expect(Add.AtNew(32).WithNew(Sam));
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
            publisher.Expect(Add.AtNew(1).WithNew(Mike));
            publisher.Expect(Add.AtNew(32).WithNew(Sam));
            publisher.Expect(Add.AtNew(1).WithNew(Tim));
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
            publisher.Expect(Add.AtNew(1).WithNew(Mike));
            publisher.Expect(Add.AtNew(32).WithNew(Sam));
            publisher.Expect(Add.AtNew(3).WithNew(Tim));
        }
    }
}