using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    [TestFixture]
    public class SelectBehaviour : TestFixture
    {
        [Test]
        public void SelectIteratorInitializeNoProjection()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorInitializeKnownProjection()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorInitializeAnonymousProjection()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorAddSingleItemNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing()
                .ThenAdd(John).ExpectNoEvents()
                .ThenInitialize().ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorAddSingleItemNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenAdd(John).ExpectEvent(Add.WithNewItems(John).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorAddSingleItemKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name}))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing()
                .ThenAdd(John).ExpectNoEvents()
                .ThenInitialize().ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorAddSingleItemKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenAdd(John).ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorAddSingleItemAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing()
                .ThenAdd(John).ExpectNoEvents()
                .ThenInitialize().ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorAddSingleItemAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenAdd(John).ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorAddMultipleItemsNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing()
                .ThenAdd(John, Mick).ExpectNoEvents()
                .ThenInitialize().ExpectNoEvents()
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorAddMultipleItemsNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenAdd(John, Mick, Simon).ExpectEvent(Add.WithNewItems(John, Mick, Simon).WithNewIndex(3))
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorAddMultipleItemsKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing()
                .ThenAdd(John, Mick).ExpectNoEvents()
                .ThenInitialize().ExpectNoEvents()
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorAddMultipleItemsKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenAdd(John, Mick, Simon).ExpectEvent(Add.WithNewItemCount(3).WithNewIndex(3))
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorAddMultipleItemsAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing()
                .ThenAdd(John, Mick).ExpectNoEvents()
                .ThenInitialize().ExpectNoEvents()
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorAddMultipleItemsAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenAdd(John, Mick, Simon).ExpectEvent(Add.WithNewItemCount(3).WithNewIndex(3))
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorRemoveSingleExistingItemNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Mike).ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }
        
        [Test]
        public void SelectIteratorRemoveSingleExistingItemNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenRemove(Mike).ExpectEvent(Remove.WithOldItems(Mike).WithOldIndex(0))
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveSingleNonExistingItemNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Clancy).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveSingleNonExistingItemNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenRemove(Clancy).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveSingleExistingItemKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Mike).ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveSingleExistingItemKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenRemove(Mike).ExpectEvent(Remove.WithOldItemCount(1).WithOldIndex(0))
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveSingleNonExistingItemKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Clancy).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveSingleNonExistingItemKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenRemove(Clancy).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveSingleExistingItemAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Mike).ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveSingleExistingItemAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenRemove(Mike).ExpectEvent(Remove.WithOldItemCount(1).WithOldIndex(0))
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveSingleNonExistingItemAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Clancy).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveSingleNonExistingItemAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded()
                .ThenRemove(Clancy).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveMultipleExistingItemsNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Mike, Tom).ExpectNoEvents()
                .AndExpectFinalCountOf(1);
        }

        [Test]
        public void SelectIteratorRemoveMultipleExistingItemsNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Mike, Tom).ExpectEvent(Remove.WithOldItems(Mike, Tom).WithOldIndex(0))
                .AndExpectFinalCountOf(1);
        }

        [Test]
        public void SelectIteratorRemoveMultipleNonExistingItemsNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Brian, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveMultipleNonExistingItemsNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Brian, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveMultiplePartlyExistingItemsNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Tom, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveMultiplePartlyExistingItemsNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Tom, Harry).ExpectEvent(Remove.WithOldItems(Tom).WithOldIndex(1))
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveMultipleExistingItemsKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Mike, Tom).ExpectNoEvents()
                .AndExpectFinalCountOf(1);
        }

        [Test]
        public void SelectIteratorRemoveMultipleExistingItemsKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Mike, Tom).ExpectEvent(Remove.WithOldItemCount(2).WithOldIndex(0))
                .AndExpectFinalCountOf(1);
        }

        [Test]
        public void SelectIteratorRemoveMultipleNonExistingItemsKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Brian, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveMultipleNonExistingItemsKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Brian, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveMultiplePartlyExistingItemsKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Tom, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveMultiplePartlyExistingItemsKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Tom, Harry).ExpectEvent(Remove.WithOldItemCount(1).WithOldIndex(1))
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveMultipleExistingItemsAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Mike, Tom).ExpectNoEvents()
                .AndExpectFinalCountOf(1);
        }

        [Test]
        public void SelectIteratorRemoveMultipleExistingItemsAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Mike, Tom).ExpectEvent(Remove.WithOldItemCount(2).WithOldIndex(0))
                .AndExpectFinalCountOf(1);
        }

        [Test]
        public void SelectIteratorRemoveMultipleNonExistingItemsAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Brian, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveMultipleNonExistingItemsAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Brian, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorRemoveMultiplePartlyExistingItemsAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenRemove(Tom, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorRemoveMultiplePartlyExistingItemsAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToUpper() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToUpper() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenRemove(Tom, Harry).ExpectEvent(Remove.WithOldItemCount(1).WithOldIndex(1))
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void SelectIteratorReplaceSingleExistingItemNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(Tom, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceSingleExistingItemNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(Tom, Harry).ExpectEvent(Replace.WithOldItems(Tom).WithNewItems(Harry).WithOldIndex(1))
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceSingleNonExistingItemNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(Rick, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceSingleNonExistingItemNoProjectionAfterInitialize()
        {
            // Special attention: since the original item didn't exist, it can't be replaced. However, 
            // the new item should be added instead
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(Rick, Harry).ExpectEvent(Add.WithNewItems(Harry).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceSingleExistingItemKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(Tom, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceSingleExistingItemKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(Tom, Harry).ExpectEvent(Replace.WithOldItemCount(1).WithNewItemCount(1).WithOldIndex(1))
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceSingleNonExistingItemKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(Rick, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceSingleNonExistingItemKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(Rick, Harry).ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }



        [Test]
        public void SelectIteratorReplaceSingleExistingItemAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(Tom, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceSingleExistingItemAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(Tom, Harry).ExpectEvent(Replace.WithOldItemCount(1).WithNewItemCount(1).WithOldIndex(1))
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceSingleNonExistingItemAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(Rick, Harry).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceSingleNonExistingItemAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(Rick, Harry).ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultipleExistingItemsNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Jack }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceMultipleExistingItemsNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Jack }, new Contact[] { Rick, Paul }).ExpectEvent(Replace.WithOldItems(Tom, Jack).WithNewItems(Rick, Paul).WithOldIndex(1))
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul })
                .ExpectEvent(Replace.WithOldItems(Tom).WithNewItems(Rick).WithOldIndex(1))
                .ExpectEvent(Add.WithNewItems(Paul).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithLessNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil, Ryan }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithLessNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil, Ryan }, new Contact[] { Rick, Paul })
                .ExpectEvent(Replace.WithOldItems(Tom).WithNewItems(Rick).WithOldIndex(1))
                .ExpectEvent(Add.WithNewItems(Paul).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithMoreNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul, Tim }).ExpectNoEvents()
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithMoreNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul, Jake })
                .ExpectEvent(Replace.WithOldItems(Tom).WithNewItems(Rick).WithOldIndex(1))
                .ExpectEvent(Add.WithNewItems(Paul, Jake).WithNewIndex(3))
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorReplaceMultipleNonExistingItemsNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Bubsy, Phil }, new Contact[] { Rick, Paul, Tim }).ExpectNoEvents()
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorReplaceMultipleNonExistingItemsNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Bubsy, Phil }, new Contact[] { Rick, Paul, Jake })
                .ExpectEvent(Add.WithNewItems(Rick, Paul, Jake).WithNewIndex(3))
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorReplaceMultipleExistingItemsKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Jack }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceMultipleExistingItemsKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Jack }, new Contact[] { Rick, Paul })
                .ExpectEvent(Replace.WithOldItemCount(2).WithNewItemCount(2).WithOldIndex(1))
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul })
                .ExpectEvent(Replace.WithOldItemCount(1).WithNewItemCount(1).WithOldIndex(1))
                .ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithLessKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil, Ryan }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithLessKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil, Ryan }, new Contact[] { Rick, Paul })
                .ExpectEvent(Replace.WithOldItemCount(1).WithNewItemCount(1).WithOldIndex(1))
                .ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithMoreKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul, Tim }).ExpectNoEvents()
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithMoreKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul, Jake })
                .ExpectEvent(Replace.WithOldItemCount(1).WithNewItemCount(1).WithOldIndex(1))
                .ExpectEvent(Add.WithNewItemCount(2).WithNewIndex(3))
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorReplaceMultipleNonExistingItemsKnownProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Bubsy, Phil }, new Contact[] { Rick, Paul, Tim }).ExpectNoEvents()
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorReplaceMultipleNonExistingItemsKnownProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Bubsy, Phil }, new Contact[] { Rick, Paul, Jake })
                .ExpectEvent(Add.WithNewItemCount(3).WithNewIndex(3))
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorReplaceMultipleExistingItemsAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Jack }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceMultipleExistingItemsAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Jack }, new Contact[] { Rick, Paul })
                .ExpectEvent(Replace.WithOldItemCount(2).WithNewItemCount(2).WithNewIndex(1))
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul })
                .ExpectEvent(Replace.WithOldItemCount(1).WithNewItemCount(1).WithNewIndex(1))
                .ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithLessAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil, Ryan }, new Contact[] { Rick, Paul }).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithLessAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil, Ryan }, new Contact[] { Rick, Paul })
                .ExpectEvent(Replace.WithOldItemCount(1).WithNewItemCount(1).WithNewIndex(1))
                .ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(3))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithMoreAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul, Tim }).ExpectNoEvents()
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorReplaceMultiplePartlyExistingItemsWithMoreAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Tom, Phil }, new Contact[] { Rick, Paul, Jake })
                .ExpectEvent(Replace.WithOldItemCount(1).WithNewItemCount(1).WithNewIndex(1))
                .ExpectEvent(Add.WithNewItemCount(2).WithNewIndex(3))
                .AndExpectFinalCountOf(5);
        }

        [Test]
        public void SelectIteratorReplaceMultipleNonExistingItemsAnonymousProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenReplace(new Contact[] { Bubsy, Phil }, new Contact[] { Rick, Paul, Tim }).ExpectNoEvents()
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorReplaceMultipleNonExistingItemsAnonymousProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.Substring(0, 1) }))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenReplace(new Contact[] { Bubsy, Phil }, new Contact[] { Rick, Paul, Jake })
                .ExpectEvent(Add.WithNewItemCount(3).WithNewIndex(3))
                .AndExpectFinalCountOf(6);
        }

        [Test]
        public void SelectIteratorMoveSingleExistingItemNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenMove(2, Mike).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorMoveSingleExistingItemNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenMove(2, Mike)
                .ExpectEvent(Move.WithOldItemCount(1).WithNewIndex(2).WithOldIndex(0))
                .AndExpectFinalCountOf(3);
        }

        [Test]
        public void SelectIteratorMoveSingleNonExistingItemNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenMove(2, Gordon).ExpectNoEvents()
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorMoveSingleNonExistingItemNoProjectionAfterInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenMove(2, Gordon).ExpectEvent(Add.WithNewItemCount(1).WithNewIndex(2))
                .AndExpectFinalCountOf(4);
        }

        [Test]
        public void SelectIteratorMoveSingleExistingItemPastRangeNoProjectionBeforeInitialize()
        {
            Given.Collection(Mike, Tom, Jack)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
                .AndLinqEquivalent(inputs => inputs)
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenMove(12, Mike).ExpectNoEvents()
                .AndExpectFinalCountOf(3);
        }

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemPastRangeNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(12, Mike).ExpectEvent(Move, 1, 2, 0)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemPastRangeNoProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(12, Gordon).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemPastRangeNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(12, Gordon).ExpectEvent(Add, 1, 3)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(2, Mike).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(2, Mike).ExpectEvent(Move, 1, 2, 0)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(2, Gordon).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(2, Gordon).ExpectEvent(Add, 1, 2)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemPastRangeKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(12, Mike).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemPastRangeKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(12, Mike).ExpectEvent(Move, 1, 2, 0)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemPastRangeKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(12, Gordon).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemPastRangeKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(12, Gordon).ExpectEvent(Add, 1, 3)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(2, Mike).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(2, Mike).ExpectEvent(Move, 1, 2, 0)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(2, Gordon).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(2, Gordon).ExpectEvent(Add, 1, 2)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemPastRangeAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(12, Mike).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleExistingItemPastRangeAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(12, Mike).ExpectEvent(Move, 1, 2, 0)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemPastRangeAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(12, Gordon).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveSingleNonExistingItemPastRangeAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(12, Gordon).ExpectEvent(Add, 1, 3)
        //        .AndExpectFinalCountOf(4);
        //}














        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsWithinRangeNoProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(2, Mike, Tom).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsWithinRangeNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(2, Mike, Tom).ExpectEvent(Move, 1).ExpectEvent(Move, 1)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsPastRangeNoProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(16, Mike, Tom).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsPastRangeNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(16, Mike, Tom).ExpectEvent(Move, 1).ExpectEvent(Move, 1)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsWithinRangeNoProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(0, Rick, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsWithinRangeNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(0, Rick, Jarryd).ExpectEvent(Add, 2, 0)
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsPastRangeNoProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(16, Rick, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsPastRangeNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(16, Rick, Jarryd).ExpectEvent(Add, 2, 3)
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsWithinRangeNoProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(1, Tom, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsWithinRangeNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(1, Tom, Jarryd).ExpectEvent(Move, 1, 1, 1).ExpectEvent(Add, 1, 2)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsPastRangeNoProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(42, Tom, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsPastRangeNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(42, Tom, Jarryd).ExpectEvent(Move, 1, 2, 1).ExpectEvent(Add, 1, 3)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsWithinRangeKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(2, Mike, Tom).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsWithinRangeKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(2, Mike, Tom).ExpectEvent(Move, 1).ExpectEvent(Move, 1)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsPastRangeKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(16, Mike, Tom).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsPastRangeKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(16, Mike, Tom).ExpectEvent(Move, 1).ExpectEvent(Move, 1)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsWithinRangeKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(0, Rick, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsWithinRangeKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(0, Rick, Jarryd).ExpectEvent(Add, 2, 0)
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsPastRangeKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(16, Rick, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsPastRangeKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(16, Rick, Jarryd).ExpectEvent(Add, 2, 3)
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsWithinRangeKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(1, Tom, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsWithinRangeKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(1, Tom, Jarryd).ExpectEvent(Move, 1, 1, 1).ExpectEvent(Add, 1, 2)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsPastRangeKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(42, Tom, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsPastRangeKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(42, Tom, Jarryd).ExpectEvent(Move, 1, 2, 1).ExpectEvent(Add, 1, 3)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsWithinRangeAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(2, Mike, Tom).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsWithinRangeAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(2, Mike, Tom).ExpectEvent(Move, 1).ExpectEvent(Move, 1)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsPastRangeAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(16, Mike, Tom).ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleExistingItemsPastRangeAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(16, Mike, Tom).ExpectEvent(Move, 1).ExpectEvent(Move, 1)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsWithinRangeAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(0, Rick, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsWithinRangeAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(0, Rick, Jarryd).ExpectEvent(Add, 2, 0)
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsPastRangeAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(16, Rick, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultipleNonExistingItemsPastRangeAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(16, Rick, Jarryd).ExpectEvent(Add, 2, 3)
        //        .AndExpectFinalCountOf(5);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsWithinRangeAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(1, Tom, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsWithinRangeAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(1, Tom, Jarryd).ExpectEvent(Move, 1, 1, 1).ExpectEvent(Add, 1, 2)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsPastRangeAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenMove(42, Tom, Jarryd).ExpectNoEvents()
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorMoveMultiplePartlyExistingItemsPastRangeAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenMove(42, Tom, Jarryd).ExpectEvent(Move, 1, 2, 1).ExpectEvent(Add, 1, 3)
        //        .AndExpectFinalCountOf(4);
        //}

        //[Test]
        //public void SelectIteratorRefreshNoProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenRefresh().ExpectNoEvents()
        //        .ThenRefresh().ExpectNoEvents()
        //        .ThenInitialize().ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorRefreshNoProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select())
        //        .AndLinqEquivalent(inputs => inputs)
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorRefreshKnownProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenRefresh().ExpectNoEvents()
        //        .ThenRefresh().ExpectNoEvents()
        //        .ThenInitialize().ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorRefreshKnownProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new ContactSummary() { Summary = c.Name.ToLower() }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorRefreshAnonymousProjectionBeforeInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WithoutInitializing().ExpectNoEvents()
        //        .ThenRefresh().ExpectNoEvents()
        //        .ThenRefresh().ExpectNoEvents()
        //        .ThenInitialize().ExpectNoEvents()
        //        .AndExpectFinalCountOf(3);
        //}

        //[Test]
        //public void SelectIteratorRefreshAnonymousProjectionAfterInitialize()
        //{
        //    Given.Collection(Mike, Tom, Jack)
        //        .WithSyncLinqQuery(inputs => inputs.AsBindable().Select(c => new { Summary = c.Name.ToLower() }))
        //        .AndLinqEquivalent(inputs => inputs.Select(c => new { Summary = c.Name.ToLower() }))
        //        .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
        //        .WhenLoaded().ExpectNoEvents()
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .ThenRefresh().ExpectEvent(Reset)
        //        .AndExpectFinalCountOf(3);
        //}
    }
}
