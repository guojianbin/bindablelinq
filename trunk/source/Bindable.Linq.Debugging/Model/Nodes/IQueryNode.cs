using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BindingOriented.SyncLinq.Debugging.Model.Events;

namespace BindingOriented.SyncLinq.Debugging.Model.Nodes
{
    internal interface IQueryNode : IDumpable
    {
        Type Type { get; }
        IEnumerable<IQueryNode> ChildNodes { get; }
        IEnumerable<IQueryNodeEvent> Events { get; }
    }
}