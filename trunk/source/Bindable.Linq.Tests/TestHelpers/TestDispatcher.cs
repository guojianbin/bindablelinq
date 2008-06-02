using System;

namespace Bindable.Linq.Tests.TestHelpers
{
    using System.Threading;
    using Threading;

    public class TestDispatcher : IDispatcher
    {
        #region IDispatcher Members
        public void Invoke(Action actionToInvoke)
        {
            var t = new Thread(delegate(object ignored) { actionToInvoke(); });
            t.Start();
        }
        #endregion
    }
}