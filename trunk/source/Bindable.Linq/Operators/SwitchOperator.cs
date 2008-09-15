using System.Collections.Generic;
using System.Linq;
using Bindable.Linq.Interfaces;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Operators
{
    /// <summary>
    /// The Operator created when a Switch is set up.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal sealed class SwitchOperator<TSource, TResult> : Operator<TSource, TResult>
    {
        private readonly ISwitchCase<TSource, TResult>[] _conditionalCases;
        private readonly ISwitchCase<TSource, TResult> _defaultCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchOperator&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="conditionalCases">The conditional cases.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public SwitchOperator(IBindable<TSource> source, IEnumerable<ISwitchCase<TSource, TResult>> conditionalCases, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            _conditionalCases = conditionalCases.Where(c => c.IsDefaultCase == false).ToArray();
            _defaultCase = conditionalCases.Where(c => c.IsDefaultCase == true).FirstOrDefault();
        }

        /// <summary>
        /// When overridden in a derived class, refreshes the operator.
        /// </summary>
        protected override void RefreshOverride()
        {
            var result = default(TResult);
            var source = Source.Current;

            var caseMatched = false;
            foreach (var conditionalCase in _conditionalCases)
            {
                if (conditionalCase.Evaluate(source))
                {
                    result = conditionalCase.Return(source);
                    caseMatched = true;
                    break;
                }
            }

            if (!caseMatched && _defaultCase != null)
            {
                result = _defaultCase.Return(source);
            }

            Current = result;
        }
    }
}
