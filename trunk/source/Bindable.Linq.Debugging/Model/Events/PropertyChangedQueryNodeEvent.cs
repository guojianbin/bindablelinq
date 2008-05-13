using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace BindingOriented.SyncLinq.Debugging.Model.Events
{
    internal class PropertyChangedQueryNodeEvent : IQueryNodeEvent
    {
        private PropertyChangedEventArgs _args;
        private DateTime _time;

        public PropertyChangedQueryNodeEvent(PropertyChangedEventArgs args)
        {
            _args = args;
            _time = DateTime.Now;
        }

        public string Description
        {
            get { return _args.PropertyName; }
        }

        public string Name
        {
            get { return "PropertyChanged"; }
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
