using System.Linq;
using NUnit.Framework;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{

    /// <summary>
    /// This class contains tests for the Bindable LINQ WhereIterator class.
    /// </summary>
    [TestFixture]
    public class WhereBehaviour : TestFixture
    {
        [Test]
        public void WhereIteratorAddMultipleItemsFailingConditonAfterInitialize()
        {
            Given.Collection(Tom).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(0).ThenAdd(Jack, Jake).ExpectNoEvents().AndExpectFinalCountOf(0);
        }

        [Test]
        public void WhereIteratorAddMultipleItemsPartlyPassingConditonAfterInitialize()
        {
            Given.Collection(Tom).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(0).ThenAdd(Jack, Sally, Tim, Sam).ExpectEvent(Add.WithNewItems(Sally, Sam).WithNewIndex(0)).AndExpectFinalCountOf(2);
        }

        [Test]
        public void WhereIteratorAddMultipleItemsPassingConditonAfterInitialize()
        {
            Given.Collection(Tom).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(0).ThenAdd(Sally, Sam).ExpectEvent(Add.WithNewItems(Sally, Sam).WithNewIndex(0)).AndExpectFinalCountOf(2);
        }

        [Test]
        public void WhereIteratorAddSingleItemFailingConditonAfterInitialize()
        {
            Given.Collection(Tom, Sally, Sam).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(2).ThenAdd(Jack).ExpectNoEvents().AndExpectFinalCountOf(2);
        }

        [Test]
        public void WhereIteratorAddSingleItemPassingConditonAfterInitialize()
        {
            Given.Collection(Tom, Sally).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(1).ThenAdd(Sam).ExpectEvent(Add.WithNewItems(Sam).WithNewIndex(1)).AndExpectFinalCountOf(2);
        }

        [Test]
        public void WhereIteratorChangeItemCausesReevaluate()
        {
            var sally = new Contact {Name = "Sally"};

            Given.Collection(Tom, sally, Jack).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(1).ThenChange(items => sally.Name = "Paul").ExpectEvent(Remove.WithOldItems(sally).WithOldIndex(0)).AndExpectFinalCountOf(0);
        }

        [Test]
        public void WhereIteratorRemoveMultipleItemsPartlyPassingConditonAfterInitialize()
        {
            Given.Collection(Tom, Sally, Jack, Sam).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(2).ThenRemove(Sally, Sam, Jack).ExpectEvent(Remove.WithOldItems(Sally).WithOldIndex(0)).ExpectEvent(Remove.WithOldItems(Sam).WithOldIndex(0)).AndExpectFinalCountOf(0);
        }

        [Test]
        public void WhereIteratorRemoveSingleItemFailingConditonAfterInitialize()
        {
            Given.Collection(Tom, Sally, Jack, Sam).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(2).ThenRemove(Jack).ExpectNoEvents().AndExpectFinalCountOf(2);
        }

        [Test]
        public void WhereIteratorRemoveSingleItemPassingConditonAfterInitialize()
        {
            Given.Collection(Tom, Sally, Jack, Sam).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(2).ThenRemove(Sam).ExpectEvent(Remove.WithOldItems(Sam).WithOldIndex(1)).AndExpectFinalCountOf(1);
        }

        [Test]
        public void WhereIteratorRemoveSingleNonExistingItemPassingConditonAfterInitialize()
        {
            Given.Collection(Tom, Sally, Jack).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(1).ThenRemove(Sam).ExpectNoEvents().AndExpectFinalCountOf(1);
        }

        [Test]
        public void WhereIteratorReplaceMultipleItemsPartlyPassingConditonAfterInitialize()
        {
            Given.Collection(Tom, Sally, Jack).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(1).ThenReplace(new[] {Sally, Jack}, new[] {Tom, Sam}).ExpectEvent(Remove.WithOldItems(Sally).WithOldIndex(0)).ExpectEvent(Add.WithNewItems(Sam).WithNewIndex(0)).AndExpectFinalCountOf(1);
        }

        [Test]
        public void WhereIteratorReplaceMultipleItemsPartlyPassingConditonAfterInitialize2()
        {
            Given.Collection(Tom, Sally, Jack).WithSyncLinqQuery(inputs => inputs.AsBindable().Where(c => c.Name.StartsWith("S"))).AndLinqEquivalent(inputs => inputs.Where(c => c.Name.StartsWith("S"))).ExpectingTheyAre(CompatibilityExpectation.FullyCompatible).WhenLoaded().ExpectNoEvents().AndCountOf(1).ThenReplace(new[] {Sally}, new[] {Sam, Sally}).ExpectEvent(Replace.WithOldItems(Sally).WithNewItems(Sam).WithOldIndex(0)).ExpectEvent(Add.WithNewItems(Sally).WithNewIndex(1)).AndExpectFinalCountOf(2);
        }
    }
}