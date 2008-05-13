using System;
using System.Collections.Generic;
using System.Text;
using BindingOriented.SyncLinq.Debugging.Model.Events;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;

namespace BindingOriented.SyncLinq.Debugging.Model.Nodes
{
    internal class IteratorQueryNode : IQueryNode
    {
        private IBindableCollection _iterator;
        private ObservableCollection<IQueryNodeEvent> _events;
        private IBindableCollection<IQueryNode> _childNodes;

        public IteratorQueryNode(IBindableCollection query)
        {
            _iterator = query;
            _events = new ObservableCollection<IQueryNodeEvent>();
            _iterator.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
                { 
                    _events.Add(new NotifyCollectionChangedQueryNodeEvent(e));
                };
            _iterator.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                _events.Add(new PropertyChangedQueryNodeEvent(e));
            };
            _childNodes = _iterator.AsBindable<object>()
                                   .Select(c => QueryNodeFactory.Create(c));
        }

        public Type Type
        {
            get { return _iterator.GetType(); }
        }

        public IEnumerable<IQueryNodeEvent> Events
        {
            get { return _events; }
        }

        public IEnumerable<IQueryNode> ChildNodes
        {
            get { return _childNodes; }
        }

        public string DumpInformation()
        {
            using (StringWriter sw = new StringWriter())
            {
                ObjectDumper.Write(_iterator, 3, sw);
                return sw.ToString();
            }
        }
    }
}
