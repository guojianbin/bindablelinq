using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections;
using Bindable.Linq.TestApplication;

namespace Bindable.Linq.TestApplication.Framework
{
    public abstract class PerformanceTestRun<TInput> : TestRun
    {
        private int _startingItems;
        private int _additionalItems;
        private ObservableCollection<TInput> _inputs;
        private NotifyCollectionChangedAction _action;

        public PerformanceTestRun(int startingItems, int additionalItems, NotifyCollectionChangedAction action)
        {
            _startingItems = startingItems;
            _additionalItems = additionalItems;
            _action = action;
            _inputs = new ObservableCollection<TInput>();
        }

        protected ObservableCollection<TInput> Inputs 
        {
            get 
            {
                return _inputs;
            }
        }

        protected int AdditionalItems
        {
            get { return _additionalItems; }
        }

        protected int StartingItems
        {
            get { return _startingItems; }
        }

        protected abstract void Initialize();
        protected abstract void PerformAction(NotifyCollectionChangedAction action);

        protected void Enumerate(IEnumerable query)
        {
            int count = 0;
            foreach (var item in query)
            {
                if (item.ToString() != "blah")
                {
                    count++;
                }
            }
            if (count == 0)
            {
                throw new InvalidOperationException();
            }
        }

        protected override void ExecuteOverride()
        {
            this.Inputs.Clear();
            this.Description = "Initializing Query";
            this.Initialize();
            this.Description = "Performing additional action";
            this.PerformAction(_action);

            this.Description = string.Format("Start with {0}", _startingItems);
            if (_action != NotifyCollectionChangedAction.Reset)
            {
                this.Description += string.Format(", then {0} {1}", _action.ToString(), _additionalItems);
            }
        }
    }
}
