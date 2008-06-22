using System;
using Bindable.Linq.Helpers;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Unit.Helpers
{
    /// <summary>
    /// This class contains tests for the Bindable LINQ Weak Event implementation.
    /// </summary>
    [TestFixture]
    public sealed class WeakEventReferenceTests
    {
        #region Test Helpers
        private sealed class EventPublisher
        {
            public int Subscribers;
            private event EventHandler _eventRaised;

            public event EventHandler EventRaised
            {
                add
                {
                    Subscribers++;
                    _eventRaised += value;
                }
                remove
                {
                    Subscribers--;
                    _eventRaised -= value;
                }
            }
        }

        private sealed class EventSubscriber
        {
            private readonly EventPublisher _publisher;

            public EventSubscriber(EventPublisher publisher)
            {
                _publisher = publisher;
                _publisher.EventRaised += Publisher_EventRaised;
            }

            ~EventSubscriber()
            {
                _publisher.EventRaised -= Publisher_EventRaised;
            }

            private void Publisher_EventRaised(object sender, EventArgs e) { }
        }

        private sealed class WeakEventSubscriber
        {
            private readonly EventHandler<EventArgs> _eventHandler;
            private readonly EventPublisher _publisher;
            private readonly WeakEventReference<EventArgs> _weakEventReference;

            public WeakEventSubscriber(EventPublisher publisher)
            {
                _publisher = publisher;
                // Create the event handlers. Note that these must be kept as member-level references,
                // so that they are coupled to the class lifetime rather than the current scope - or else
                // no one would reference the event handler (since the WeakEventReference just keeps 
                // a weak reference to it)!
                _eventHandler = EventPublisher_EventRaised;
                _weakEventReference = new WeakEventReference<EventArgs>(_eventHandler);
                _publisher.EventRaised += _weakEventReference.WeakEventHandler;
            }

            private void EventPublisher_EventRaised(object sender, EventArgs e) { }

            ~WeakEventSubscriber()
            {
                // Unhook the event. The publisher will then no longer reference the WeakEventReference
                // either, and so that object will also be cleared - the end result is that we have cleaned 
                // after ourselves completely. NB: In a standard implementation you should also 
                // put this call in a Dispose() method.
                _publisher.EventRaised -= _weakEventReference.WeakEventHandler;
            }
        }

        private WeakReference CreateEventSubscriber(EventPublisher publisher)
        {
            var subscriber = new EventSubscriber(publisher);
            Assert.AreEqual(1, publisher.Subscribers);
            return new WeakReference(subscriber);
        }

        private WeakReference CreateWeakEventSubscriber(EventPublisher publisher)
        {
            var subscriber = new WeakEventSubscriber(publisher);
            Assert.AreEqual(1, publisher.Subscribers);
            return new WeakReference(subscriber);
        }
        #endregion

        /// <summary>
        /// Tests that standard .NET events do indeed cause referencing issues between 
        /// short-lived objects subscribing to events on long-lived objects.
        /// </summary>
        [Test]
        public void WeakEventReferenceStandardEventIntroducesLeaks()
        {
            // Create an event publisher (a long-lived object) and an event subscriber
            // using standard .NET events. We only have a weak reference to the subscriber, 
            // so the only other GC reference will be the publisher - this is what introduces
            // memory issues in binding and WPF applications.
            var publisher = new EventPublisher();
            var subscriberReference = CreateEventSubscriber(publisher);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Prove that the subscriber is being kept alive by the publisher's reference back to it,
            // and that it is still subscribed (even though it unsubscribes in the finalizer - so it can't 
            // have been finalized).
            Assert.IsNotNull(subscriberReference.Target);
            Assert.AreEqual(1, publisher.Subscribers);
        }

        /// <summary>
        /// Tests that the Weak Event Reference implementation in Bindable LINQ solves the problem 
        /// shown in the above test.
        /// </summary>
        [Test]
        public void WeakEventReferenceWeakEventsFixLeaks()
        {
            // Create an event publisher (a long-lived object) and an event subscriber
            // using our custom weak event handler code. We will only have a weak reference 
            // to the created subscriber, and since the weak event handler should remove the 
            // reference from the publisher to the subscriber, the subscriber should be 
            // marked for collection and finalized.
            var publisher = new EventPublisher();
            var subscriberReference = CreateWeakEventSubscriber(publisher);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Prove that the weak event reference worked - the subscriber should not have been 
            // referenced from the publishers' event handler, and since we have no other normal 
            // references to it, it should have been collected and the finalizer should have 
            // unhooked the event handler.
            Assert.IsNull(subscriberReference.Target);
            Assert.AreEqual(0, publisher.Subscribers);
        }
    }
}