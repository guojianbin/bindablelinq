using System;
using System.Collections.Generic;

namespace Bindable.Linq.Interfaces.Internal.Events
{
    internal delegate void EvaluatingEventHandler<TElement>(object sender, EvaluatingEventArgs<TElement> args);

    internal class EvaluatingEventArgs<TElement> : EventArgs
    {
        private List<TElement> _itemsYeildedFromFirstEvaluation;
    }
}