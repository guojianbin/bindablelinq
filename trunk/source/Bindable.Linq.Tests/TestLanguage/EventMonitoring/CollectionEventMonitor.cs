using System.Collections.Specialized;

namespace Bindable.Linq.Tests.TestLanguage.EventMonitoring
{
    /// <summary>
    /// Represents an event monitor for monitoring events raised by collections through <see cref="INotifyCollectionChanged"/>.
    /// </summary>
    internal sealed class CollectionEventMonitor : EventMonitor<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionEventMonitor"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public CollectionEventMonitor(INotifyCollectionChanged publisher)
            : base(publisher)
        {
            
        }

        /// <summary>
        /// Subscribes to the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected override void Subscribe(INotifyCollectionChanged publisher)
        {
            publisher.CollectionChanged += new NotifyCollectionChangedEventHandler(this.RecordEvent);
        }

        /// <summary>
        /// Unsubscribes from the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected override void Unsubscribe(INotifyCollectionChanged publisher)
        {
            publisher.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.RecordEvent);
        }
    }
}
