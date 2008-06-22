using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Bindable.Linq.Tests.TestLanguage.Expectations
{
    internal sealed class CurrentCountExpectation : IExpectation
    {
        private int _count;

        public CurrentCountExpectation(int count)
        {
            _count = count;
        }

        public void Validate(IScenario scenario)
        {
            IBindableQuery query = scenario.BindableLinqQuery as IBindableQuery;
            if (query == null)
            {
                throw new NotSupportedException("The query is not of type IBindableQuery, and so the CurrentCount property cannot be checked");
            }
            int count = query.CurrentCount;
            Assert.AreEqual(_count, count);
        }
    }
}
