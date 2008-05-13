using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Bindable.Linq.Tests.TestHelpers
{
    internal class PropertyEventCatcher : EventCatcher<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        public PropertyEventCatcher(INotifyPropertyChanged publisher)
            : base(publisher)
        {
        }

        protected override void Subscribe(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged += new PropertyChangedEventHandler(Publisher_PropertyChanged);
        }

        protected override void Unsubscribe(INotifyPropertyChanged publisher)
        {
            publisher.PropertyChanged -= new PropertyChangedEventHandler(Publisher_PropertyChanged);
        }

        private void Publisher_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RecordEvent(sender, e);
        }
    }
}
