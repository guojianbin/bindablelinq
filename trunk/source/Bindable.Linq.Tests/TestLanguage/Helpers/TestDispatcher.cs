using System;
using System.Threading;
using Bindable.Linq.Threading;

namespace Bindable.Linq.Tests.TestLanguage.Helpers
{
    /// <summary>
    /// Represents an <see cref="IDispatcher"/> used for tests. Since test frameworks
    /// do not have message pumps normally used by dispatchers, the event is simply 
    /// invoked on a new thread.
    /// </summary>
    internal class TestDispatcher : IDispatcher
    {
        #region IDispatcher Members
        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        public void Invoke(Action actionToInvoke)
        {
            var t = new Thread(ignored => actionToInvoke());
            t.Start();
        }
        #endregion
    }
}