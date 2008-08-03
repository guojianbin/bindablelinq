using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bindable.Linq.Collections;
using Bindable.Linq.Tests.TestLanguage.EventMonitoring;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using Bindable.Linq.Tests.TestLanguage.Steps;
using NUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Represents a scenario that is defined as part of a specification.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    internal class Scenario<TInput> : IScenario<TInput>
    {
        private string _title;
        private BindableCollection<TInput> _inputs;
        private IBindableCollection _bindableLinqQuery;
        private IEnumerable _standardLinqQuery;
        private IEnumerable<Step> _steps;
        private CollectionEventMonitor _eventMonitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario&lt;TInput&gt;"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="inputs">The inputs.</param>
        /// <param name="steps">The steps.</param>
        /// <param name="bindableLinqQuery">The bindable linq query.</param>
        /// <param name="standardLinqQuery">The standard linq query.</param>
        public Scenario(string title, BindableCollection<TInput> inputs, IEnumerable<Step> steps, IBindableCollection bindableLinqQuery, IEnumerable standardLinqQuery)
        {
            _title = title;
            _inputs = inputs;
            _steps = steps.ToList();
            _bindableLinqQuery = bindableLinqQuery;
            _standardLinqQuery = standardLinqQuery;
            _eventMonitor = new CollectionEventMonitor(_bindableLinqQuery);
            foreach (var step in _steps)
            {
                step.Scenario = this;
            }
        }

        /// <summary>
        /// Gets the inputs to the scenario.
        /// </summary>
        public BindableCollection<TInput> Inputs
        {
            get { return _inputs; }
        }

        /// <summary>
        /// Gets the instance of the Bindable LINQ query.
        /// </summary>
        public IBindableCollection BindableLinqQuery
        {
            get { return _bindableLinqQuery; }
        }

        /// <summary>
        /// Gets the instance of the standard LINQ query.
        /// </summary>
        public IEnumerable StandardLinqQuery
        {
            get { return _standardLinqQuery; }
        }

        /// <summary>
        /// Gets the title of the specification.
        /// </summary>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets an event monitor attached to the collection.
        /// </summary>
        public CollectionEventMonitor EventMonitor
        {
            get { return _eventMonitor; }
        }

        /// <summary>
        /// Executes the scenario and verifies the expectations attached to it.
        /// </summary>
        public void Execute()
        {
            Tracer.WriteLine("Validating scenario: {0}", this.Title);
            int stepCount = 1;
            using (Tracer.Indent())
            {
                foreach (var step in _steps)
                {
                    Tracer.Write("Step {0} ", stepCount);
                    try
                    {
                        step.Execute();
                        Tracer.WriteLine("");
                    }
                    catch
                    {
                        Tracer.WriteLine("failed");
                        Tracer.WriteLine("");
                        Tracer.WriteLine("Exception details:");
                        throw;
                    }
                    stepCount++;
                }

                // Ensure no additional events are in the queue
                Tracer.WriteLine("Finalizing: Verifying additional events were not raised.");
                Assert.IsNull(EventMonitor.DequeueNextEvent(), "The test has completed, but there are still events in the queue that were not expected.");

                // Compare with LINQ to Objects
                Tracer.WriteLine("Finalizing: Comparing final results to LINQ to Objects");
                CompatibilityValidator.CompareWithLinq(CompatabilityLevel.FullyCompatible, BindableLinqQuery, StandardLinqQuery);

                // Forget all references to the query and ensure it is garbage collected
                Tracer.WriteLine("Finalizing: Detecting memory leaks and event handlers that have not been unhooked.");
                WeakReference bindableQuery = new WeakReference(this.BindableLinqQuery, false);
                _bindableLinqQuery = null;
                _eventMonitor.Dispose();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                Assert.IsFalse(bindableQuery.IsAlive, "There should be no live references to the bindable query at this point. This may indicate that the query or items within the query have event handlers that have not been unhooked.");
            }
        }
    }
}
