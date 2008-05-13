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
    public class CollectionChangePublisherTests : TestFixture
    {
        [Test]
        public void SenderIsImpersonated()
        {
            bool eventWasRaised = false;
            object foo = new object();

            CollectionChangedPublisher<Contact> publisher = new CollectionChangedPublisher<Contact>(foo);
            publisher.CollectionChanged += new NotifyCollectionChangedEventHandler(
                delegate(object sender, NotifyCollectionChangedEventArgs e)
                {
                    eventWasRaised = true;
                    Assert.IsTrue(object.ReferenceEquals(sender, foo));
                });

            using (var recorder = publisher.Record())
            {
                Assert.IsFalse(eventWasRaised);
                recorder.RecordAdd(new Contact(), 3);
                Assert.IsFalse(eventWasRaised);
            }
            Assert.IsTrue(eventWasRaised);
        }
    }
}
