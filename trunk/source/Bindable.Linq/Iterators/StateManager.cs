using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq;
using Bindable.Linq.Helpers;
using Bindable.Linq.Collections;

namespace Bindable.Linq.Iterators
{
    /// <summary>
    /// This class provides the ability to track the state of an item in a collection, as well 
    /// as to iterate over the items in a thread-safe manner. 
    /// </summary>
    internal sealed class StateManager<TElement, TState>
        where TState : struct
        where TElement : class
    {
        private readonly BindableCollection<TElement> _elements = new BindableCollection<TElement>();
        private readonly Dictionary<TElement, TState> _elementStateLookup = new Dictionary<TElement, TState>();
        private readonly LockScope _stateManagerLock = new LockScope();

        /// <summary>
        /// Initializes a new instance of the <see cref="StateManager&lt;TElement, TState&gt;"/> class.
        /// </summary>
        public StateManager()
        {
        }

        /// <summary>
        /// Sets the state for a given element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="state">The state.</param>
        public void SetState(TElement element,
            TState state)
        {
            using (_stateManagerLock.Enter(this))
            {
                if (!_elementStateLookup.ContainsKey(element))
                {
                    _elementStateLookup.Add(element, state);
                    _elements.Add(element);
                }
                else
                {
                    _elementStateLookup[element] = state;
                }
            }
        }

        /// <summary>
        /// Removes an element (and forgets its state) from this <see cref="T:StateManager"/>.
        /// </summary>
        /// <param name="element"></param>
        public void Remove(TElement element)
        {
            using (_stateManagerLock.Enter(this))
            {
                if (_elementStateLookup.ContainsKey(element))
                {
                    _elementStateLookup.Remove(element);
                    _elements.Remove(element);
                }
            }
        }

        /// <summary>
        /// Gets the state of a given element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public TState? GetState(TElement element)
        {
            TState? result = null;
            if (element != null)
            {
                using (_stateManagerLock.Enter(this))
                {
                    if (_elementStateLookup.ContainsKey(element))
                    {
                        result = _elementStateLookup[element];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all of the elements with a given state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public IEnumerator<TElement> GetAllInState(TState? state)
        {
            List<TElement> results = new List<TElement>();
            using (_stateManagerLock.Enter(this))
            {
                foreach (KeyValuePair<TElement, TState> pair in _elementStateLookup)
                {
                    if (state == null || pair.Value.Equals(state.Value))
                    {
                        results.Add(pair.Key);
                    }
                }
            }
            return results.GetEnumerator();
        }
    }
}