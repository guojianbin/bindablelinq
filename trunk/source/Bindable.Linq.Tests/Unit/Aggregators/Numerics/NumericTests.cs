using System;

namespace Bindable.Linq.Tests.Unit.NumericAggregates
{
    using NUnit.Framework;

    /// <summary>
    /// Contains unit tests for the various <see cref="T:INumeric`2"/> implementations.
    /// </summary>
    [TestFixture]
    public class NumericTests
    {
        private void Test<T, A>(Func<int, T> c, Func<int, A> a, Func<IBindableCollection<T>, IBindable<T>> sum, Func<IBindableCollection<T>, IBindable<A>> average, Func<IBindableCollection<T>, IBindable<T>> min, Func<IBindableCollection<T>, IBindable<T>> max)
        {
            // Test the Sum operations
            Assert.AreEqual(c(27), sum(new object[] {c(7), c(8), c(12)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(-78), sum(new object[] {c(13), c(9), c(-100)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(15), sum(new object[] {c(14), c(1), c(0)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(0), sum(new object[] {c(0), c(0)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(0), sum(new object[] {}.AsBindable<object, T>()).Current);
            if (typeof (T).Name.Contains("Nulla"))
            {
                Assert.AreEqual(c(0), sum(new object[] {(T) (object) null, (T) (object) null}.AsBindable<object, T>()).Current);
                Assert.AreEqual(c(-14), sum(new object[] {(T) (object) null, c(-14)}.AsBindable<object, T>()).Current);
                Assert.AreEqual(c(14), sum(new object[] {(T) (object) null, c(14)}.AsBindable<object, T>()).Current);
            }

            // Test the Average operations
            Assert.AreEqual(a(8), average(new object[] {c(7), c(8), c(9)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(a(-10), average(new object[] {c(-20), c(-10), c(0)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(a(0), average(new object[] {c(-14), c(0), c(14)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(a(0), average(new object[] {c(0), c(0)}.AsBindable<object, T>()).Current);
            if (typeof (T).Name.Contains("Nulla"))
            {
                Assert.AreEqual((T) (object) null, average(new object[] {}.AsBindable<object, T>()).Current);
                Assert.AreEqual((T) (object) null, average(new object[] {(T) (object) null, (T) (object) null}.AsBindable<object, T>()).Current);
                Assert.AreEqual(a(-14), average(new object[] {(T) (object) null, c(-14)}.AsBindable<object, T>()).Current);
                Assert.AreEqual(a(14), average(new object[] {(T) (object) null, c(14)}.AsBindable<object, T>()).Current);
            }
            else
            {
                Assert.AreEqual(a(0), average(new object[] {}.AsBindable<object, T>()).Current);
            }

            // Test the Min operations
            Assert.AreEqual(c(7), min(new object[] {c(7), c(8), c(9)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(-20), min(new object[] {c(-20), c(-10), c(0)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(-14), min(new object[] {c(-14), c(0), c(14)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(0), min(new object[] {c(0), c(0)}.AsBindable<object, T>()).Current);
            if (typeof (T).Name.Contains("Nulla"))
            {
                Assert.AreEqual((T) (object) null, min(new object[] {}.AsBindable<object, T>()).Current);
                Assert.AreEqual((T) (object) null, min(new object[] {(T) (object) null, (T) (object) null}.AsBindable<object, T>()).Current);
                Assert.AreEqual(c(-14), min(new object[] {(T) (object) null, c(-14)}.AsBindable<object, T>()).Current);
                Assert.AreEqual(c(14), min(new object[] {(T) (object) null, c(14)}.AsBindable<object, T>()).Current);
            }
            else
            {
                Assert.AreEqual(c(0), min(new object[] {}.AsBindable<object, T>()).Current);
            }

            // Test the Max operations
            Assert.AreEqual(c(9), max(new object[] {c(7), c(8), c(9)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(0), max(new object[] {c(-20), c(-10), c(0)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(14), max(new object[] {c(-14), c(0), c(14)}.AsBindable<object, T>()).Current);
            Assert.AreEqual(c(0), max(new object[] {c(0), c(0)}.AsBindable<object, T>()).Current);
            if (typeof (T).Name.Contains("Nulla"))
            {
                Assert.AreEqual((T) (object) null, max(new object[] {}.AsBindable<object, T>()).Current);
                Assert.AreEqual((T) (object) null, max(new object[] {(T) (object) null, (T) (object) null}.AsBindable<object, T>()).Current);
                Assert.AreEqual(c(-14), max(new object[] {(T) (object) null, c(-14)}.AsBindable<object, T>()).Current);
                Assert.AreEqual(c(14), max(new object[] {(T) (object) null, c(14)}.AsBindable<object, T>()).Current);
            }
            else
            {
                Assert.AreEqual(c(0), max(new object[] {}.AsBindable<object, T>()).Current);
            }
        }

        /// <summary>
        /// Tests the Add method of the <see cref="T:DecimalNumeric"/> class.
        /// </summary>
        [Test]
        public void DecimalTests()
        {
            Test<decimal, decimal>(i => i, i => i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
            Test<decimal?, decimal?>(i => i, i => i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
        }

        /// <summary>
        /// Tests the <see cref="T:DoubleNumeric"/> class.
        /// </summary>
        [Test]
        public void DoubleTests()
        {
            Test<double, double>(i => i, i => i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
            Test<double?, double?>(i => i, i => i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
        }

        /// <summary>
        /// Tests the <see cref="T:FloatNumeric"/> class.
        /// </summary>
        [Test]
        public void FloatTests()
        {
            Test<float, float>(i => i, i => i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
            Test<float?, float?>(i => i, i => i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
        }

        /// <summary>
        /// Tests the <see cref="T:Int32Numeric"/> class.
        /// </summary>
        [Test]
        public void Int32Tests()
        {
            Test(i => i, i => (double) i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
            Test<int?, double?>(i => i, i => (double) i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
        }

        /// <summary>
        /// Tests the <see cref="T:Int64Numeric"/> class.
        /// </summary>
        [Test]
        public void Int64AddTests()
        {
            Test<long, double>(i => i, i => (double) i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
            Test<long?, double?>(i => i, i => (double) i, c => c.Sum(), c => c.Average(), c => c.Min(), c => c.Max());
        }
    }
}