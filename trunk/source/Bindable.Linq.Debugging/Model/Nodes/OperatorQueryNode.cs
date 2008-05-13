using System;
using System.Collections.Generic;
using System.Text;
using BindingOriented.SyncLinq.Debugging.Model.Events;
using System.IO;

namespace BindingOriented.SyncLinq.Debugging.Model.Nodes
{
    internal class OperatorQueryNode : IQueryNode
    {
        private IBindable _operator;

        public OperatorQueryNode(IBindable query)
        {
            _operator = query;
        }

        public Type Type
        {
            get { return _operator.GetType(); }
        }

        public IEnumerable<IQueryNode> ChildNodes
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IQueryNodeEvent> Events
        {
            get { throw new NotImplementedException(); }
        }

        public string DumpInformation()
        {
            return ObjectDumper.Write(_operator);
        }
    }
}
