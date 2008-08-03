using System.Linq;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using NUnit.Framework;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.MockObjectModel;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// This class contains tests for the Bindable LINQ WhereIterator class.
    /// </summary>
    [TestFixture]
    public class WhereBehaviour : TestFixture
    {
        [Test]
        public void WhereSpecification()
        {
            Specification.Title("Where() specification")
                .TestingOver<Contact>()
                .UsingBindableLinq(inputs => inputs.AsBindable().Where(p => p.Name.Length >= 4))
                .UsingStandardLinq(inputs => inputs.Where(p => p.Name.Length >= 4))
                .Scenario("Delayed evaluation",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Construction().ItWill.NotHaveEvaluated(),
                    step => Upon.Reading(q => q.CurrentCount).ItWill.NotHaveEvaluated(),
                    step => Upon.Reading(q => q.Configuration).ItWill.NotHaveEvaluated(),
                    step => Upon.Evaluate().ItWill.HaveCount(2)
                    )
                .Scenario("Adding items",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Add(John, Mick).ItWill.NotRaiseAnything(),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Add(Jarryd).ItWill.Raise(Add.With(Jarryd).At(4)),
                    step => Upon.Add(Tim).ItWill.NotRaiseAnything(),
                    step => Upon.Add(Tom, Sally).ItWill.Raise(Add.With(Sally).At(5)),
                    step => Upon.Insert(2, Simon).ItWill.Raise(Add.With(Simon).At(2)),
                    //step => Upon.Insert(3, Phil, Jake).ItWill.Raise(Add.With(Phil, Jake).At(3)),
                    //step => Upon.Insert(3, Rick, Ryan, Tim).ItWill.Raise(Add.With(Rick, Ryan).At(3)),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything()
                    )
                //.Scenario("Moving items",
                //    With.Inputs(Mike, Tom, Jack, John, Sally, Sam, Tim, Clancy),
                //    step => Upon.Evaluate().ItWill.NotRaiseAnything().And.HaveCount(5),
                //    step => Upon.Move(3, Mike).ItWill.Raise(Move.With(Mike).AtNew(3).AtOld(0)),
                //    step => Upon.Move(3, Tom).ItWill.NotRaiseAnything(),
                //    step => Upon.Move(4, Jack, John).ItWill.Raise(Move.With(Jack).AtNew(4).AtOld(0)).And.Raise(Move.With(John).AtNew(4).AtOld(0))
                //    )
                //.Scenario("Moving non-consecutive items",
                //    With.Inputs(Mike, Tom, Jack, John, Sally, Sam, Tim, Clancy),
                //    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                //    step => Upon.Move(6, Mike, John).ItWill.Raise(Move.With(projected(Mike)).AtNew(6).AtOld(0)).And.Raise(Move.With(projected(John)).AtNew(7).AtOld(2))
                //    )
                //.Scenario("Removing items",
                //    With.Inputs(Mike, Tom, Jack, John, Sally, Sam, Tim, Clancy),
                //    step => Upon.Remove(Jack).ItWill.NotRaiseAnything(),
                //    step => Upon.Evaluate().ItWill.NotRaiseAnything().And.HaveCount(7),
                //    step => Upon.Remove(John).ItWill.Raise(Remove.With(projected(John)).At(2)),
                //    step => Upon.Remove(Sally, Sam).ItWill.Raise(Remove.With(projected(Sally), projected(Sam)).At(2)),
                //    step => Upon.Remove(Mike, Clancy).ItWill.Raise(Remove.With(projected(Mike)).At(0)).And.Raise(Remove.With(projected(Clancy)).At(3))
                //    )
                //.Scenario("Replacing items",
                //    With.Inputs(Mike, Tom, Jack, Clancy),
                //    step => Upon.Replace(Mike).With(Sally).ItWill.NotRaiseAnything(),
                //    step => Upon.Evaluate(),
                //    step => Upon.Replace(Jack).With(Sam).ItWill.Raise(Replace.WithOld(projected(Jack)).WithNew(projected(Sam))),
                //    step => Upon.Replace(Sally, Tom).With(Gordon, Sue).ItWill.Raise(Replace.WithOld(projected(Sally), projected(Tom)).WithNew(projected(Gordon), projected(Sue)))
                //    )
                //.Scenario("Replacing non-consecutive items",
                //    With.Inputs(Mike, Tom, Jack, Clancy),
                //    step => Upon.Evaluate(),
                //    step => Upon.Replace(Mike, Clancy).With(Gordon, Sue).ItWill.Raise(Replace.WithOld(projected(Mike)).WithNew(projected(Gordon))).And.Raise(Replace.WithOld(projected(Clancy)).WithNew(projected(Sue)))
                //    )
                //.Scenario("Replacing item that isn't in source",
                //    With.Inputs(Mike, Tom, Jack),
                //    step => Upon.Evaluate(),
                //    step => Upon.Replace(Mike, Clancy).With(Gordon, Sue).ItWill.Raise(Replace.WithOld(projected(Mike)).WithNew(projected(Gordon))).And.Raise(Add.With(projected(Sue)))
                //    )
                .Verify();
        }
    }
}