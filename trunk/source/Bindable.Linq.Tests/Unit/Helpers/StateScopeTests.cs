using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies;
using NUnit.Framework;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Tests.Unit.Helpers
{
    /// <summary>
    /// This class contains unit tests for the <see cref="T:StateScope" />.
    /// </summary>
    [TestFixture]
    public sealed class StateScopeTests
    {
        /// <summary>
        /// Tests that entering a scope multiple times puts it into the state.
        /// </summary>
        [Test]
        public void StateScopeEntranceTest()
        {
            StateScope scope = new StateScope();
            Assert.AreEqual(false, scope.IsWithin);
            using (scope.Enter())
            {
                Assert.AreEqual(true, scope.IsWithin);
                using (scope.Enter())
                {
                    Assert.AreEqual(true, scope.IsWithin);
                }
                Assert.AreEqual(true, scope.IsWithin);
            }
            Assert.AreEqual(false, scope.IsWithin);
        }

        /// <summary>
        /// Tests that entering a scope multiple times raises events.
        /// </summary>
        [Test]
        public void StateScopeEntranceTriggersCallback()
        {
            int eventsRaised = 0;
            StateScopeChangedCallback callback = new StateScopeChangedCallback(
                delegate
                {
                    eventsRaised++;
                });

            StateScope scope = new StateScope(callback);
            Assert.AreEqual(0, eventsRaised);
            using (scope.Enter())
            {
                // Went from false to true - so it should have raised
                Assert.AreEqual(1, eventsRaised);
                using (scope.Enter())
                {
                    // Already in - no raise
                    Assert.AreEqual(1, eventsRaised);
                }
                // Still in - no raise
                Assert.AreEqual(1, eventsRaised);
            }
            // Left - raise
            Assert.AreEqual(2, eventsRaised);
        }
    }
}
