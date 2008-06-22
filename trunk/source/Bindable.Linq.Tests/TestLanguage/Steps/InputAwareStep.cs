namespace Bindable.Linq.Tests.TestLanguage.Steps
{
    using System;
    using Collections;
    using System.Diagnostics;

    internal sealed class InputAwareStep<TInput> : Step
    {
        private readonly Action<BindableCollection<TInput>> _action;
        private readonly Expectations<InputAwareStep<TInput>> _itWill;

        public InputAwareStep(Action<BindableCollection<TInput>> action)
        {
            _action = action;
            _itWill = new Expectations<InputAwareStep<TInput>>(this);
        }

        public Expectations<InputAwareStep<TInput>> And
        {
            get { return _itWill; }
        }

        [DebuggerNonUserCode()]
        public Expectations<InputAwareStep<TInput>> ItWill
        {
            get { return _itWill; }
        }

        protected override void ExecuteOverride()
        {
            _action(((IScenario<TInput>) Scenario).Inputs);
        }
    }
}