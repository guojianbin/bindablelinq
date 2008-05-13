using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

namespace BindingOriented.SyncLinq.Debugging.Model.Events
{
    internal class NotifyCollectionChangedQueryNodeEvent : IQueryNodeEvent
    {
        private NotifyCollectionChangedEventArgs _args;
        private DateTime _time;

        public NotifyCollectionChangedQueryNodeEvent(NotifyCollectionChangedEventArgs args)
        {
            _args = args;
            _time = DateTime.Now;
        }

        public string Description
        {
            get { return _args.Action.ToString(); }
        }

        public string Name
        {
            get { return "CollectionChanged"; }
        }

        public DateTime Time
        {
            get { return _time; }
        }

        public string DumpInformation()
        {
            return ObjectDumper.Write(_args);
        }
    }
}
