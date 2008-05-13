using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BindingOriented.SyncLinq.Debugging.Model.Nodes
{
    internal static class QueryNodeFactory
    {
        public static IQueryNode Create(object queryNode)
        {
            IQueryNode result = null;
            if (queryNode is IQuery)
            {
                QueryDescriptor descriptor = ((IQuery) queryNode).GetDescriptor();
                switch (descriptor.QueryType)
                {
                    case QueryType.Iterator:
                        result = new IteratorQueryNode(queryNode as IBindableCollection);
                        break;
                    case QueryType.Aggregator:
                        result = new AggregatorQueryNode(queryNode as IBindable);
                        break;
                    case QueryType.Operator:
                        result = new OperatorQueryNode(queryNode as IBindable);
                        break;
                }
            }
            return result;
        }
    }
}
