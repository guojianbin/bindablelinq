using System;
using System.Linq;
using System.Linq.Expressions;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// Contains high-level behavioral tests for the Select iterator. 
    /// </summary>
    [TestFixture]
    public class SelectBehavior : TestFixture
    {
        /// <summary>
        /// Executes the select specification for a variety of variations in projections using the Select statement.
        /// </summary>
        [Test]
        public void SelectSpecifications()
        {
            ExecuteSelectSpecification("Select() with no projection", contact => contact);
            ExecuteSelectSpecification("Select() with anonymous projection", contact => new { BlahBlah = contact.Name });
            ExecuteSelectSpecification("Select() with anonymous projection", contact => new { DudeName = contact.Name.ToUpper() });
            //ExecuteSelectSpecification("Select() with known projection", contact => new ContactSummary() { Summary = contact.Name.ToUpper() });
        }

        /// <summary>
        /// The select specification.
        /// </summary>
        public void ExecuteSelectSpecification<TProjectionResult>(string title, Expression<Func<Contact, TProjectionResult>> selector)
        {
            var p = selector.Compile();
            Specification.Title(title)
                .TestingOver<Contact>()
                .UsingBindableLinq(inputs => inputs.AsBindable().Select(selector))
                .UsingStandardLinq(inputs => inputs.Select(p))
                .Scenario("Delayed evaluation",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Construction().ItWill.NotHaveEvaluated(),
                    step => Upon.Evaluate().ItWill.HaveCount(3)
                    )
                .Scenario("Adding items",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Add(John, Mick).ItWill.NotRaiseAnything(),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Add(Jarryd).ItWill.Raise(Add.With(p(Jarryd)).At(5)),
                    step => Upon.Add(Tom, Sally).ItWill.Raise(Add.With(p(Tom)).At(6)).And.Raise(Add.With(p(Sally)).At(7)),
                    step => Upon.Insert(2, Simon).ItWill.Raise(Add.With(p(Simon)).At(2)),
                    step => Upon.Insert(3, Phil, Jake).ItWill.Raise(Add.With(p(Phil)).At(3)).And.Raise(Add.With(p(Jake)).At(4)),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything()
                    )
                .Scenario("Moving items",
                    With.Inputs(Mike, Tom, Jack, John, Sally, Sam, Tim, Clancy),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Move(5, Mike).ItWill.Raise(Move.With(p(Mike)).AtNew(5).AtOld(0)),
                    step => Upon.Move(6, Jack, John).ItWill.Raise(Move.With(p(Jack)).AtNew(6).AtOld(1)).And.Raise(Move.With(p(John)).AtNew(7).AtOld(1))
                    )
                .Scenario("Moving non-consecutive items",
                    With.Inputs(Mike, Tom, Jack, John, Sally, Sam, Tim, Clancy),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything(),
                    step => Upon.Move(6, Mike, John).ItWill.Raise(Move.With(p(Mike)).AtNew(6).AtOld(0)).And.Raise(Move.With(p(John)).AtNew(7).AtOld(2))
                    )
                .Scenario("Removing items",
                    With.Inputs(Mike, Tom, Jack, John, Sally, Sam, Tim, Clancy),
                    step => Upon.Remove(Jack).ItWill.NotRaiseAnything(),
                    step => Upon.Evaluate().ItWill.NotRaiseAnything().And.HaveCount(7),
                    step => Upon.Remove(John).ItWill.Raise(Remove.With(p(John)).At(2)),
                    step => Upon.Remove(Sally, Sam).ItWill.Raise(Remove.With(p(Sally)).At(2)).And.Raise(Remove.With(p(Sam)).At(3)),
                    step => Upon.Remove(Mike, Clancy).ItWill.Raise(Remove.With(p(Mike)).At(0)).And.Raise(Remove.With(p(Clancy)).At(3))
                    )
                .Scenario("Replacing items",
                    With.Inputs(Mike, Tom, Jack, Clancy),
                    step => Upon.Replace(Mike).With(Sally).ItWill.NotRaiseAnything(),
                    step => Upon.Evaluate(),
                    step => Upon.Replace(Jack).With(Sam).ItWill.Raise(Replace.WithOld(p(Jack)).WithNew(p(Sam))),
                    step => Upon.Replace(Sally, Tom).With(Gordon, Sue).ItWill.Raise(Replace.WithOld(p(Sally), p(Tom)).WithNew(p(Gordon), p(Sue)))
                    )
                .Scenario("Replacing non-consecutive items",
                    With.Inputs(Mike, Tom, Jack, Clancy),
                    step => Upon.Evaluate(),
                    step => Upon.Replace(Mike, Clancy).With(Gordon, Sue).ItWill.Raise(Replace.WithOld(p(Mike)).WithNew(p(Gordon))).And.Raise(Replace.WithOld(p(Clancy)).WithNew(p(Sue)))
                    )
                .Scenario("Replacing item that isn't in source",
                    With.Inputs(Mike, Tom, Jack),
                    step => Upon.Evaluate(),
                    step => Upon.Replace(Mike, Clancy).With(Gordon, Sue).ItWill.Raise(Replace.WithOld(p(Mike)).WithNew(p(Gordon))).And.Raise(Add.With(p(Sue)))
                    )
                .Verify();
            // TODO: Add scenarios common workflows like adding and removing an item, and edge cases
        }
    }
}