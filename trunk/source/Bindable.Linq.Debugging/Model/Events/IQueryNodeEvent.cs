using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BindingOriented.SyncLinq.Debugging.Model.Events
{
    internal interface IQueryNodeEvent : IDumpable
    {
        string Description { get; }
        string Name { get; }
        DateTime Time { get; }
    }
}
