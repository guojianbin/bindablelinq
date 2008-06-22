using System;
using System.Collections.Generic;
using System.Linq;
using Bindable.Linq.Collections;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using Bindable.Linq.Tests.TestLanguage.Steps;

namespace Bindable.Linq.Tests.TestLanguage.Specifications
{
    /// <summary>
    /// A specification in which both the input and output types are known.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    internal class FullyTypedSpecification<TInput, TResult>
    {
        private string _title;
        private Func<IEnumerable<TInput>, IBindableCollection<TResult>> _bindableLinqQueryCreator;
        private Func<IEnumerable<TInput>, IEnumerable<TResult>> _standardLinqQueryCreator;
        private List<Scenario<TInput>> _scenarios = new List<Scenario<TInput>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FullyTypedSpecification&lt;TInput, TResult&gt;"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="outputQueryCreator">The output query creator.</param>
        public FullyTypedSpecification(string title, Func<IEnumerable<TInput>, IBindableCollection<TResult>> outputQueryCreator)
        {
            _title = title;
            _bindableLinqQueryCreator = outputQueryCreator;
        }

        /// <summary>
        /// Specifices the standard LINQ to Objects query that the results will be compared with.
        /// </summary>
        /// <param name="queryCreator">The query creator.</param>
        /// <returns></returns>
        public FullyTypedSpecification<TInput, TResult> UsingStandardLinq(Func<IEnumerable<TInput>, IEnumerable<TResult>> queryCreator)
        {
            _standardLinqQueryCreator = queryCreator;
            return this;
        }

        /// <summary>
        /// Defines a scenario for this specification.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="inputs">The inputs.</param>
        /// <param name="steps">The steps to perform.</param>
        /// <returns></returns>
        public FullyTypedSpecification<TInput, TResult> Scenario(string title, BindableCollection<TInput> inputs, params Func<object, Step>[] steps)
        {
            var scenario = new Scenario<TInput>(
                title,
                inputs,
                steps.Select(step => step(null)),
                _bindableLinqQueryCreator(inputs),
                _standardLinqQueryCreator(inputs));
            _scenarios.Add(scenario);
            return this;
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Verify()
        {
            Tracer.WriteLine("Verifying specification: {0}", _title);
            using (Tracer.Indent())
            {
                foreach (var scenario in _scenarios)
                {
                    scenario.Execute();
                }
            }
        }
    }
}
