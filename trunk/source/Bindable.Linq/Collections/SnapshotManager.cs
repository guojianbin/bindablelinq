using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Collections
{
    /// <summary>
    /// Manages the snapshots of a collection.
    /// </summary>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    internal sealed class SnapshotManager<TElement>
    {
        private readonly object _snapshotManagerLock = new object();
        private List<TElement> _latestSnapshot;
        private Func<List<TElement>> _rebuildCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotManager&lt;TElement&gt;"/> class.
        /// </summary>
        /// <param name="rebuildCallback">The rebuild callback.</param>
        public SnapshotManager(Func<List<TElement>> rebuildCallback)
        {
            _rebuildCallback = rebuildCallback;
        }

        /// <summary>
        /// Invalidates this instance.
        /// </summary>
        public void Invalidate()
        {
            lock (_snapshotManagerLock)
            {
                _latestSnapshot = null;
            }
        }

        /// <summary>
        /// Creates the enumerator.
        /// </summary>
        public IEnumerator<TElement> CreateEnumerator()
        {
            lock (_snapshotManagerLock)
            {
                if (_latestSnapshot == null)
                {
                    _latestSnapshot = _rebuildCallback();
                }
                return _latestSnapshot.GetEnumerator();
            }
        }
    }
}
