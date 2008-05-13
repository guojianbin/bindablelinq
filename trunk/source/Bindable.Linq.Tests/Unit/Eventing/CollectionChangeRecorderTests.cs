using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Bindable.Linq.Eventing;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Unit.Eventing
{
    [TestFixture]
    public class CollectionChangeRecorderTests : TestFixture
    {
        [Test]
        public void NonAdjacentAddsAreNotCombined()
        {
            MockPublisher publisher = new MockPublisher();
            using (var recorder = new CollectionChangeRecorder<Contact>(publisher))
            {
                recorder.RecordAdd(Mike, 1);
                recorder.RecordAdd(Sam, 32);
                recorder.RecordAdd(Tim, 3);
                publisher.ExpectNoEvents();
            }
            publisher.Expect(Add.WithNewIndex(1).WithNewItems(Mike));
            publisher.Expect(Add.WithNewIndex(32).WithNewItems(Sam));
            publisher.Expect(Add.WithNewIndex(3).WithNewItems(Tim));
        }

        [Test]
        public void AdjacentAddsAreCombined()
        {
            MockPublisher publisher = new MockPublisher();
            using (var recorder = new CollectionChangeRecorder<Contact>(publisher))
            {
                recorder.RecordAdd(Mike, 1);
                recorder.RecordAdd(Sam, 32);
                recorder.RecordAdd(Tim, 2);
                publisher.ExpectNoEvents();
            }
            publisher.Expect(Add.WithNewIndex(1).WithNewItems(Mike, Tim));
            publisher.Expect(Add.WithNewIndex(32).WithNewItems(Sam));
        }

        [Test]
        public void MatchingAddsAreNotCombined()
        {
            MockPublisher publisher = new MockPublisher();
            using (var recorder = new CollectionChangeRecorder<Contact>(publisher))
            {
                recorder.RecordAdd(Mike, 1);
                recorder.RecordAdd(Sam, 32);
                recorder.RecordAdd(Tim, 1);
                publisher.ExpectNoEvents();
            }
            publisher.Expect(Add.WithNewIndex(1).WithNewItems(Mike));
            publisher.Expect(Add.WithNewIndex(32).WithNewItems(Sam));
            publisher.Expect(Add.WithNewIndex(1).WithNewItems(Tim));
        }

        private class MockPublisher : ICollectionChangedPublisher<Contact>
        {
            private Queue<NotifyCollectionChangedEventArgs> _arguments;

            public MockPublisher()
            {
                _arguments = new Queue<NotifyCollectionChangedEventArgs>();
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;

            public void Raise(NotifyCollectionChangedEventArgs e)
            {
                _arguments.Enqueue(e);
            }

            public ICollectionChangedRecorder<Contact> Record()
            {
                throw new NotImplementedException();
            }

            public void Expect(CollectionChangeSpecification specification)
            {
                if (_arguments.Count > 0)
                {
                    NotifyCollectionChangedEventArgs lastEvent = _arguments.Dequeue();
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
    }
}
