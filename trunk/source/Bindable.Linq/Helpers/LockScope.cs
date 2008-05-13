using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using Bindable.Linq.Helpers;
using System.Diagnostics;

namespace Bindable.Linq.Helpers
{
    /// <summary>
    /// A helper class for acquiring <see cref="Monitor"/> locks using a specified timeout 
    /// (defaulting to 3 seconds). 
    /// </summary>
    public sealed class LockScope : IDisposable
    {
        private readonly object _clientLock = new object();
        private readonly object _innerLock = new object();
        private readonly int _lockAttemptTimeoutSeconds = 0;
        private object _currentHolder;
        private DateTime _currentHolderAcquiredTime;
        private Thread _currentHolderThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockScope"/> class.
        /// </summary>
        public LockScope()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockScope"/> class.
        /// </summary>
        /// <param name="lockAttemptTimeoutSeconds">The timeout, in seconds, in which the lock
        /// must be acquired before an exception is thrown.</param>
        public LockScope(int lockAttemptTimeoutSeconds)
        {
            _lockAttemptTimeoutSeconds = lockAttemptTimeoutSeconds;
        }

        /// <summary>
        /// Attempts to acquire the lock.
        /// </summary>
        public LockScope Enter(object requestingHolder)
        {
            requestingHolder.ShouldNotBeNull("requestingHolder");
            lock (_innerLock)
            {
                if (_lockAttemptTimeoutSeconds > 0)
                {
                    if (!Monitor.TryEnter(_clientLock, _lockAttemptTimeoutSeconds * 10000))
                    {
                        Thread currentHolderThread = _currentHolderThread;
                        string currentHolderString = (_currentHolder ?? string.Empty).ToString();
                        Thread requestingHolderThread = Thread.CurrentThread;
                        string requestingHolderString = requestingHolder.ToString();
                        double holdLength = 0;
                        if (_currentHolderAcquiredTime != DateTime.MinValue)
                        {
                            holdLength = DateTime.Now.Subtract(_currentHolderAcquiredTime).TotalSeconds;
                        }

                        if (currentHolderThread != null)
                        {
                            throw new LockAttemptTimeoutException(string.Format(CultureInfo.InvariantCulture, "Lock could not be acquired by object '{0}' on thread '{1}' (ID {2}) within a {3} second timeout period. The lock is currently held by '{4}' on thread '{5}' (ID {6}) and has been held for {7:n2} seconds. This may indicate a possible deadlock situation.", requestingHolderString, requestingHolderThread, requestingHolderThread.ManagedThreadId, _lockAttemptTimeoutSeconds, currentHolderString, currentHolderThread.Name, currentHolderThread.ManagedThreadId, holdLength));
                        }
                        else
                        {
                            throw new LockAttemptTimeoutException(string.Format(CultureInfo.InvariantCulture, "Lock could not be acquired by object '{0}' on thread '{1}' (ID {2}) within a {3} second timeout period, although there is no current holder. This may mean that the lock was released shortly after the timeout.", requestingHolderString, requestingHolderThread, requestingHolderThread.ManagedThreadId, _lockAttemptTimeoutSeconds));
                        }
                    }
                    else
                    {
                        _currentHolder = requestingHolder;
                        _currentHolderThread = Thread.CurrentThread;
                        _currentHolderAcquiredTime = DateTime.Now;
                    }
                }
                else
                {
                    Monitor.Enter(_clientLock);
                    _currentHolder = requestingHolder;
                    _currentHolderThread = Thread.CurrentThread;
                    _currentHolderAcquiredTime = DateTime.Now;
                }
            }
            return this;
        }

        /// <summary>
        /// Releases the lock.
        /// </summary>
        public void Leave()
        {
            lock (_innerLock)
            {
                _currentHolder = null;
                _currentHolderThread = null;
                _currentHolderAcquiredTime = DateTime.MinValue;
                Monitor.Exit(_clientLock);
            }
        }

        /// <summary>
        /// Asserts that the lock is not currently held by the current thread.
        /// </summary>
        public void MustNotBeHeld()
        {
            #if DEBUG
            #if !SILVERLIGHT
            string message = null;
            lock (_innerLock)
            {
                if (_currentHolder != null)
                {
                    message = "Lock {0} should not be held, but is currently held by {1}#{2} on thread {3}".FormatWith(
                        this.GetHashCode(),
                        _currentHolder.ToString(),
                        _currentHolder.GetHashCode(),
                        _currentHolderThread.ManagedThreadId
                        );
                }
            }
            if (message != null)
            {
                Debug.Fail(message);
            }
            #endif
            #endif
        }

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            Leave();
        }
        #endregion
    }
}