using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    internal sealed class CountExpectation : IExpectation
    {
        private int _count;

        public CountExpectation(int count)
        {
            _count = count;
        }

        public void Validate(IScenario scenario)
        {
            int count = scenario.BindableLinqQuery.Count;
            Assert.AreEqual(_count, count);
        }
    }
}
