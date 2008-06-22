using System.ComponentModel;

namespace Bindable.Linq.Tests.TestLanguage.EventMonitoring
{
    /// <summary>
    /// An event monitor for properties via <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    internal sealed class PropertyEventMonitor : EventMonitor<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEventMonitor"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public PropertyEventMonitor(INotifyPropertyChanged publisher)
            : base(publisher)
        {
            
        }

        /// <summary>
        /// Subscribes to the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected override void Subscribe(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged += new PropertyChangedEventHandler(this.RecordEvent);
        }

        /// <summary>
        /// Unsubscribes from the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        protected override void Unsubscribe(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged -= new PropertyChangedEventHandler(this.RecordEvent);
        }
    }
}
