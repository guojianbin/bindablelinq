using System;
using System.Linq.Expressions;

namespace Bindable.Linq.Operators
{
    public sealed class SwitchCase<TInput, TResult> : ISwitchCase<TInput, TResult>
    {
        private readonly Expression<Func<TInput, bool>> _inputCondition;
        private readonly Expression<Func<TInput, TResult>> _resultExpression;
        private readonly Func<TInput, bool> _inputConditionCompiled;
        private readonly Func<TInput, TResult> _resultExpressionCompiled;
        private readonly bool _isDefaultCase;

        public SwitchCase(Expression<Func<TInput, bool>> inputCondition, Expression<Func<TInput, TResult>> resultExpression, bool isDefaultCase)
        {
            _inputCondition = inputCondition;
            _resultExpression = resultExpression;
            _inputConditionCompiled = _inputCondition.Compile();
            _resultExpressionCompiled = _resultExpression.Compile();
            _isDefaultCase = isDefaultCase;
        }

        public bool Evaluate(TInput input)
        {
            return _inputConditionCompiled(input);
        }

        public TResult Return(TInput input)
        {
            return _resultExpressionCompiled(input);
        }

        public bool IsDefaultCase
        {
            get { return _isDefaultCase; }
        }
    }
}
