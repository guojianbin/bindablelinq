namespace Bindable.Linq.Tests.TestHelpers
{
    using System.ComponentModel;

    internal class PropertyEventCatcher : EventCatcher<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        public PropertyEventCatcher(INotifyPropertyChanged publisher)
            : base(publisher) {}

        protected override void Subscribe(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged += Publisher_PropertyChanged;
        }

        protected override void Unsubscribe(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged -= Publisher_PropertyChanged;
        }

        private void Publisher_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RecordEvent(sender, e);
        }
    }
}