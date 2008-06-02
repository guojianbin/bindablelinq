using System;
using System.Linq;
using NUnit.Framework;
using Bindable.Linq.Tests.TestHelpers;

namespace Bindable.Linq.Tests.Behaviour.Dependencies
{

    [TestFixture]
    public sealed class WhereDependenciesBehavior : TestFixture
    {
        [Test]
        public void Test1()
        {
            Given.Collection(Mike, Sue, Lesley)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.ToLower().Contains("s")))
                .AndLinqEquivalent(inputs => inputs.Where(c => c.Name.ToLower().Contains("s")))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }
    }
}