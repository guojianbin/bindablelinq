using System;
using System.Threading;
using Bindable.Linq.Threading;
using System.Collections.Generic;

namespace Bindable.Linq.Tests.TestLanguage.Helpers
{
    /// <summary>
    /// Represents an <see cref="IDispatcher"/> used for tests. Since test frameworks
    /// do not have message pumps normally used by dispatchers, the event is simply 
    /// invoked on a new thread.
    /// </summary>
    internal class TestDispatcher : IDispatcher
    {
        private readonly List<Thread> _dispatcherThreads = new List<Thread>();
        private readonly Thread _creationThread = Thread.CurrentThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDispatcher"/> class.
        /// </summary>
        public TestDispatcher()
        {
        }

        /// <summary>
        /// Dispatches the specified action to the thread.
        /// </summary>
        /// <param name="actionToInvoke">The action to invoke.</param>
        public void Dispatch(Action actionToInvoke)
        {
            if (DispatchRequired())
            {
                var reset = new AutoResetEvent(false);
                var thread = new Thread(
                    ignored =>
                        {
                            actionToInvoke();
                            reset.Set();
                        });
                _dispatcherThreads.Add(thread);
                thread.Start();
                reset.WaitOne();
            }
            else
            {
                actionToInvoke();
            }
        }

        /// <summary>
        /// Dispatches the specified action to invoke.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="actionToInvoke">The action to invoke.</param>
        /// <returns></returns>
        public TResult Dispatch<TResult>(Func<TResult> actionToInvoke)
        {
            if (DispatchRequired())
            {
                var result = default(TResult);
                var reset = new AutoResetEvent(false);
                var thread = new Thread(
                    ignored =>
                        {
                            result = actionToInvoke();
                            reset.Set();
                        });
                _dispatcherThreads.Add(thread);
                thread.Start();
                reset.WaitOne();
                return result;
            }
            return actionToInvoke();
        }

        /// <summary>
        /// Checks whether the thread invoking the method is the dispatcher thread.
        /// </summary>
        /// <returns></returns>
        public bool DispatchRequired()
        {
            return
                Thread.CurrentThread != _creationThread
                && _dispatcherThreads.Contains(Thread.CurrentThread) == false;
        }
    }
}