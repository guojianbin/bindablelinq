﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Dependencies
{
    /// <summary>
    /// A base class for managing weak-reference event subscriptions on multiple objects,
    /// ensuring:
    /// - The event isn't subscribed twice on the one object
    /// - All subscriptions can be easily cleared
    /// </summary>
    /// <typeparam name="TPublisher">The type of event publisher.</typeparam>
    /// <typeparam name="TEventArgs">The type of event argument.</typeparam>
    internal abstract class EventDependency<TPublisher, TEventArgs> : IDisposable where TEventArgs : EventArgs
    {
        private readonly Dictionary<int, WeakReference> _observables;
        private readonly object _observablesLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventDependency&lt;TPublisher, TEventArgs&gt;"/> class.
        /// </summary>
        protected EventDependency()
        {
            _observables = new Dictionary<int, WeakReference>();
        }

        /// <summary>
        /// When overriden in a derived class, allows the class to subscribe a given event handler to 
        /// the publishing class.
        /// </summary>
        /// <param name="publisher">The event publisher.</param>
        protected abstract void AttachOverride(TPublisher publisher);

        /// <summary>
        /// When overriden in a derived class, allows the class to unsubscribe a given event handler from 
        /// the publishing class.
        /// </summary>
        /// <param name="publisher">The event publisher.</param>
        protected abstract void DetachOverride(TPublisher publisher);

        /// <summary>
        /// Attaches a range of objects to observe.
        /// </summary>
        /// <param name="range">The range of objects.</param>
        public void AttachRange(IEnumerable range)
        {
            foreach (object publisher in range)
            {
                Attach(publisher);
            }
        }

        /// <summary>
        /// Attaches an object to observe.
        /// </summary>
        /// <param name="objectToObserve">The object to be observed.</param>
        public void Attach(object objectToObserve)
        {
            if (objectToObserve is TPublisher)
            {
                TPublisher publisher = (TPublisher) objectToObserve;
                lock (_observablesLock)
                {
                    if (Find(publisher) == null)
                    {
                        _observables.Add(publisher.GetHashCode(), new WeakReference(publisher, false));
                        AttachOverride(publisher);
                    }
                }
            }
        }

        /// <summary>
        /// Detaches a range of objects that were observed.
        /// </summary>
        /// <param name="range">The range of objects.</param>
        public void DetachRange(IEnumerable range)
        {
            foreach (object publisher in range)
            {
                Detach(publisher);
            }
        }

        /// <summary>
        /// Detaches an object that was observed.
        /// </summary>
        /// <param name="objectThatWasObserved">The object that was observed.</param>
        public void Detach(object objectThatWasObserved)
        {
            Detach(objectThatWasObserved, true);
        }

        /// <summary>
        /// Detaches an object that was observed.
        /// </summary>
        /// <param name="objectThatWasObserved">The object that was observed.</param>
        /// <param name="remove">Whether to also remove the item.</param>
        public void Detach(object objectThatWasObserved, bool remove)
        {
            if (objectThatWasObserved is TPublisher)
            {
                TPublisher publisher = (TPublisher) objectThatWasObserved;
                lock (_observablesLock)
                {
                    WeakReference existingReference = Find(publisher);
                    if (existingReference != null)
                    {
                        DetachOverride(publisher);
                        if (remove)
                        {
                            _observables.Remove(publisher.GetHashCode());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears all event subscriptions.
        /// </summary>
        public void Clear()
        {
            this.Each(o => Detach(o, false));
            _observables.Clear();
        }

        private void Each(Action<TPublisher> callback)
        {
            if (_observables != null)
            {
                lock (_observablesLock)
                {
                    foreach (WeakReference observable in _observables.Values)
                    {
                        object reference = observable.Target;
                        if (reference is TPublisher)
                        {
                            TPublisher publisher = (TPublisher) reference;
                            callback(publisher);
                        }
                    }
                }
            }
        }

        private WeakReference Find(TPublisher target)
        {
            WeakReference result = null;
            if (_observables != null)
            {
                lock (_observablesLock)
                {
                    int hashcode = target.GetHashCode();
                    if (_observables.ContainsKey(hashcode))
                    {
                        result = _observables[hashcode];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// When overridden in a derived class, allows the class to add custom code when the object is disposed.
        /// </summary>
        protected abstract void DisposeOverride();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DisposeOverride();
            Clear();
        }
    }
}