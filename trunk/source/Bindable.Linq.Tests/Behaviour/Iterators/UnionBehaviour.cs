using System.Linq;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// This class contains tests for the Bindable LINQ UnionIterator class.
    /// </summary>
    [TestFixture]
    public class UnionBehaviour : TestFixture
    {
        [Test]
        public void UnionSpecification()
        {
            var additionalContacts = With.Inputs(John, Tom, Jarryd);

            Specification.Title("Union()")
                .TestingOver<Contact>()
                .UsingBindableLinq(inputs => inputs.AsBindable().Union(additionalContacts))
                .UsingStandardLinq(inputs => inputs.Union(additionalContacts))
                .Scenario("Delayed evaluation",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Construction().ItWill.NotHaveEvaluated(),
                    step => Upon.Reading(q => q.CurrentCount).ItWill.NotHaveEvaluated(),
                    step => Upon.Reading(q => q.Configuration).ItWill.NotHaveEvaluated(),
                    step => Upon.Evaluate().ItWill.HaveCount(6)
                    )
                .Scenario("Adding items",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Add(Rick, Mick).ItWill.NotRaiseAnything(),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Add(Jarryd).ItWill.Raise(Add.With(Jarryd).At(8)),
                    step => Upon.Add(Tom, Sally).ItWill.Raise(Add.With(Tom).At(9)).And.Raise(Add.With(Sally).At(10)),
                    step => Upon.Insert(2, Simon).ItWill.Raise(Add.With(Simon).At(11)),
                    step => Upon.Insert(3, Phil, Jake).ItWill.Raise(Add.With(Phil).At(12)).And.Raise(Add.With(Jake).At(13)),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything()
                    )
                .Verify();
        }
    }
}