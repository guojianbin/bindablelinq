using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BindingOriented.SyncLinq.Iterators
{
    internal sealed class SourceResultPairList<TSource, TResult> : List<KeyValuePair<TSource, TResult>>
    {
        public List<TSource> SourceItems
        {
            get { return this.Select(kvp => kvp.Key).ToList(); }
        }

        public List<TResult> ResultItems
        {
            get { return this.Select(kvp => kvp.Value).ToList(); }
        }

        public void Add(TSource source, TResult result)
        {
            this.Add(new KeyValuePair<TSource, TResult>(source, result));
        }

        public void Remove(TResult reclaimed)
        {
            if (this.ResultItems.Contains(reclaimed))
            {
                this.RemoveAt(this.ResultItems.IndexOf(reclaimed));
            }
        }
    }
}