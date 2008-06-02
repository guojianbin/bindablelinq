using System;

namespace Bindable.Linq.Tests.TestHelpers
{
    using System.Collections;
    using System.Collections.Specialized;
    using NUnit.Framework;

    internal class CollectionEventCatcher : EventCatcher<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        public CollectionEventCatcher(INotifyCollectionChanged publisher)
            : base(publisher) {}

        protected override void Subscribe(INotifyCollectionChanged publisher)
        {
            publisher.CollectionChanged += Publisher_CollectionChanged;
        }

        protected override void Unsubscribe(INotifyCollectionChanged publisher)
        {
            publisher.CollectionChanged -= Publisher_CollectionChanged;
        }

        private void SubscribeChildren(IEnumerable items)
        {
            foreach (object o in items)
            {
                if (o is INotifyCollectionChanged)
                {
                    Monitor((INotifyCollectionChanged) o);
                }
            }
        }

        private void Publisher_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Validate the event
            if (sender is IBindableQuery && e is NotifyCollectionChangedEventArgs)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        // When add events are raised from a Bindable LINQ query, an index 
                        // should always be specified
                        Assert.IsTrue(e.NewStartingIndex >= 0, "NewStartingIndex should be provided when raising an 'Add' CollectionChanged event.");
                        Assert.IsTrue(e.NewItems != null, "NewItems should not be null when raising an 'Add' CollectionChanged event.");
                        Assert.IsTrue(e.OldItems == null, "OldItems should be null when raising a 'Add' CollectionChanged event.");
                        Assert.IsTrue(e.OldStartingIndex == -1, "OldStartingIndex should be -1 when raising an 'Add' CollectionChanged event.");
                        break;
                    case NotifyCollectionChangedAction.Move:
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        Assert.IsTrue(e.NewItems == null, "NewItems should be null when raising a 'Remove' CollectionChanged event.");
                        Assert.IsTrue(e.OldItems != null, "OldItems should not be null when raising a 'Remove' CollectionChanged event.");
                        if (e.OldItems.Count == 1)
                        {
                            Assert.IsTrue(e.NewStartingIndex == -1, "NewStartingIndex should be -1 when raising a 'Remove' CollectionChanged event.");
                            Assert.IsTrue(e.OldStartingIndex >= 0, "OldStartingIndex is required when raising a 'Remove' CollectionChanged event.");
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        break;
                    default:
                        break;
                }
            }
            if (e.NewItems != null)
            {
                SubscribeChildren(e.NewItems);
            }
            RecordEvent(sender, e);
        }
    }
}