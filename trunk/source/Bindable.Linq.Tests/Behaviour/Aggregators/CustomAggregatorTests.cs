namespace Bindable.Linq.Tests.Behaviour.Aggregators
{
    using Collections;
    using NUnit.Framework;
    using TestHelpers;

    /// <summary>
    /// Contains unit tests for the <see cref="T:CustomAggregator`2"/> class.
    /// </summary>
    [TestFixture]
    public class CustomAggregatorTests
    {
        /// <summary>
        /// Tests that the aggreator calculates correctly.
        /// </summary>
        [Test]
        public void CustomAggregatorAccumulateTypeCalculate()
        {
            var names = new[] {"Paul", "Jackie", "Tom"};
            var aggregator = names.AsBindable().Aggregate(0, (i, name) => i + name.Length);
            var eventCatcher = new PropertyEventCatcher(aggregator);
            Assert.AreEqual(4 + 6 + 3, aggregator.Current);
            Assert.AreEqual(1, eventCatcher.Count);
        }

        /// <summary>
        /// Tests that the aggreator calculates correctly.
        /// </summary>
        [Test]
        public void CustomAggregatorCalculate()
        {
            var numbers = new object[] {1, 2, 4};
            var aggregator = numbers.AsBindable<object, int>().Aggregate((i, result) => result + i);
            var eventCatcher = new PropertyEventCatcher(aggregator);
            Assert.AreEqual(1 + 2 + 4, aggregator.Current);
            Assert.AreEqual(1, eventCatcher.Count);
        }

        /// <summary>
        /// Tests that the aggreator calculates correctly.
        /// </summary>
        [Test]
        public void CustomAggregatorCollectionChangeCausesRefresh()
        {
            var numbers = new BindableCollection<object>();
            numbers.AddRange(1, 2, 4);
            var aggregator = numbers.AsBindable<object, int>().Aggregate((i, result) => result + i);
            var eventCatcher = new PropertyEventCatcher(aggregator);
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