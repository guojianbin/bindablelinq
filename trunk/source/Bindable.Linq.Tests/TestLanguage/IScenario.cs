using System.Collections;
using Bindable.Linq.Collections;
using Bindable.Linq.Tests.TestLanguage.EventMonitoring;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// Represents a scenario.
    /// </summary>
    internal interface IScenario
    {
        IBindableCollection BindableLinqQuery { get; }
        IEnumerable StandardLinqQuery { get; }
        string Title { get; }
        CollectionEventMonitor EventMonitor { get; }
        void Execute();
    }

    /// <summary>
    /// Represents a scenario.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    internal interface IScenario<TInput> : IScenario
    {
        BindableCollection<TInput> Inputs { get; }
    }
}