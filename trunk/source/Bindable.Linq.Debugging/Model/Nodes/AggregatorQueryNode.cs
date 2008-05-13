using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using BindingOriented.SyncLinq.Debugging.Model.Events;
using System.Collections.ObjectModel;
using System.IO;

namespace BindingOriented.SyncLinq.Debugging.Model.Nodes
{
    internal sealed class AggregatorQueryNode : IQueryNode
    {
        private IBindable _aggregate;
        private ObservableCollection<IQueryNodeEvent> _events;
        private ObservableCollection<IQueryNode> _childNodes;

        public AggregatorQueryNode(IBindable query)
        {
            _aggregate = query;
            _events = new ObservableCollection<IQueryNodeEvent>();
            _childNodes = new ObservableCollection<IQueryNode>();
            _aggregate.PropertyChanged += new PropertyChangedEventHandler(Aggregate_PropertyChanged);
        }

        public Type Type
        {
            get { return _aggregate.GetType(); }
        }

        public IEnumerable<IQueryNode> ChildNodes
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IQueryNodeEvent> Events
        {
            get { return _events; }
        }

        private void Aggregate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _events.Add(new PropertyChangedQueryNodeEvent(e));
        }

        public string DumpInformation()
        {
            using (StringWriter sw = new StringWriter())
            {
                ObjectDumper.Write(_aggregate, 3, sw);
                return sw.ToString();
            }
        }
    }
}
