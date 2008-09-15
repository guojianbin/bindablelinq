using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Interfaces;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    class PropertyValueExpectation : IExpectation
    {
        private Func<IBindableCollection, bool> _expectation;

        public PropertyValueExpectation(Func<IBindableCollection, bool> expectation)
        {
            _expectation = expectation;
        }

        public void Validate(IScenario scenario)
        {
            _expectation(scenario.BindableLinqQuery);
        }
    }
}
