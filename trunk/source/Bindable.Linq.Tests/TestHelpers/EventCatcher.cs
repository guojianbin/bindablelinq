using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies;
using NUnit.Framework;

namespace Bindable.Linq.Tests.TestHelpers
{
    /// <summary>
    /// A helper class for testing Bindable LINQ queries.
    /// </summary>
    internal abstract class EventCatcher<TPublisher, TEventArgs> : IDisposable
        where TPublisher : class
    {
        private Dictionary<TPublisher, List<TEventArgs>> _events = new Dictionary<TPublisher, List<TEventArgs>>();
        private TPublisher _defaultItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EventCatcher&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        public EventCatcher()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCatcher&lt;TPublisher, TEventArgs&gt;"/> class.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public EventCatcher(TPublisher publisher)
        {
            Monitor(publisher);
        }

        public void Monitor(TPublisher publisher)
        {
            if (_defaultItem == null)
            {
                _defaultItem = publisher;
            }
            if (!_events.ContainsKey(publisher))
            {
                _events.Add(publisher, new List<TEventArgs>());
                Subscribe(publisher);
            }
        }

        protected abstract void Subscribe(TPublisher publisher);
        protected abstract void Unsubscribe(TPublisher publisher);

        public int Count
        {
            get
            {
                return _events[_defaultItem].Count;
            }
        }

        public TEventArgs this[int index]
        {
            get
            {
                return _events[_defaultItem][index];
            }
        }

        public List<TEventArgs> this[TPublisher publisher]
        {
            get
            {
                return _events[publisher];
            }
        }

        public TEventArgs DequeueNextEvent()
        {
            return DequeueNextEvent(_defaultItem);
        }

        public TEventArgs DequeueNextEvent(TPublisher publisher)
        {
            TEventArgs result = default(TEventArgs);
            if (_events[publisher].Count > 0)
            {
                result = _events[publisher][0];
                _events[publisher].RemoveAt(0);
            }
            return result;
        }

        protected void RecordEvent(object sender, TEventArgs e)
        {
            _events[(TPublisher)sender].Add(e);
        }

        public void Clear()
        {
            this.Unsubscribe(this._defaultItem);
            _events[this._defaultItem].Clear();
        }

        public void Clear(TPublisher publisher)
        {
            this.Unsubscribe(publisher);
            _events[publisher].Clear();
        }

        public void Dispose()
        {
            foreach (TPublisher publisher in _events.Keys)
            {
                this.Unsubscribe(publisher);
            }
            _events.Clear();
            _defaultItem = null;
        }
    }

}
