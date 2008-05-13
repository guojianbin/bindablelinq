using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Bindable.Linq.Aggregators;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Aggregators.Numerics;
using Bindable.Linq.Tests.TestHelpers;
using NUnit.Framework;
using Bindable.Linq.Helpers;
using Bindable.Linq.Collections;

namespace Bindable.Linq.Tests.Behaviour.Aggregators
{
    /// <summary>
    /// Contains unit tests for the <see cref="T:SumAggregator`1"/> class.
    /// </summary>
    [TestFixture]
    public sealed class SumAggregatorTests : TestFixture
    {
        /// <summary>
        /// Tests that the aggreator calculates correctly.
        /// </summary>
        [Test]
        public void SumAggregatorCalculate()
        {
            object[] numbers = new object[] { 1, 2, 4 };
            IBindable<int> aggregator = numbers.AsBindable<object, int>().Sum();
            PropertyEventCatcher eventCatcher = new PropertyEventCatcher(aggregator);
            Assert.AreEqual(1 + 2 + 4, aggregator.Current);
            Assert.AreEqual(1, eventCatcher.Count);
        }

        /// <summary>
        /// Tests that the aggreator calculates correctly.
        /// </summary>
        [Test]
        public void SumAggregatorCollectionChangeCausesRefresh()
        {
            BindableCollection<object> numbers = new BindableCollection<object>();
            numbers.AddRange(1, 2, 4);
            IBindable<int> aggregator = numbers.AsBindable<object, int>().Sum();
            PropertyEventCatcher eventCatcher = new PropertyEventCatcher(aggregator);
            Assert.AreEqual(1 + 2 + 4, aggregator.Current);
            Assert.AreEqual(1, eventCatcher.Count); 

            // Test an Add
            numbers.Add(8);
            Assert.AreEqual(1 + 2 + 4 + 8, aggregator.Current);
            Assert.AreEqual(2, eventCatcher.Count);

            // Test a Remove
            numbers.RemoveAt(0);
            Assert.AreEqual(2 + 4 + 8, aggregator.Current);
            Assert.AreEqual(3, eventCatcher.Count);

            // Test a Replace
            numbers[0] = 4;
            Assert.AreEqual(4 + 4 + 8, aggregator.Current);
            Assert.AreEqual(4, eventCatcher.Count);

            // Test an AddRange
            numbers.AddRange(4, 5);
            Assert.AreEqual(4 + 4 + 8 + 4 + 5, aggregator.Current);
            Assert.AreEqual(5, eventCatcher.Count);

            // Test a replace with the same value - no events should be raised
            numbers[0] = numbers[0];
            Assert.AreEqual(4 + 4 + 8 + 4 + 5, aggregator.Current);
            Assert.AreEqual(5, eventCatcher.Count);

            // Test an add with 0
            numbers.Add(0);
            Assert.AreEqual(4 + 4 + 8 + 4 + 5, aggregator.Current);
            Assert.AreEqual(5, eventCatcher.Count);
        }
    }
}
