using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// Contains unit tests for the <see cref="T:AsynchronousIterator`1"/> class.
    /// </summary>
    [TestFixture]
    public class GroupByBehaviour : TestFixture
    {
        [Test]
        public void GroupByIteratorBeforeInitialize()
        {
            Given.Collection(Tom, Tim, Sally)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().GroupBy(c => c.Name[0]))
                .AndLinqEquivalent(inputs => inputs.GroupBy(c => c.Name[0]))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void GroupByIteratorAfterInitialize()
        {
            Given.Collection(Tom, Tim, Sally)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().GroupBy(c => c.Name[0]))
                .AndLinqEquivalent(inputs => inputs.GroupBy(c => c.Name[0]))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void GroupByIteratorAddSingleItemToExistingGroupBeforeInitialize()
        {
            Given.Collection(Tom, Tim, Sally)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().GroupBy(c => c.Name[0]))
                .AndLinqEquivalent(inputs => inputs.GroupBy(c => c.Name[0]))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WithoutInitializing().ExpectNoEvents()
                .ThenAdd(Sam).ExpectNoEvents().ExpectNoEventsOnGroup(1)
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void GroupByIteratorAddSingleItemToExistingGroupAfterInitialize()
        {
            Given.Collection(Tom, Tim, Sally)
                .WithSyncLinqQuery(inputs => inputs.AsBindable().GroupBy(c => c.Name[0]))
                .AndLinqEquivalent(inputs => inputs.GroupBy(c => c.Name[0]))
                .ExpectingTheyAre(CompatibilityExpectation.FullyCompatible)
                .WhenLoaded().ExpectNoEvents()
                .ThenAdd(Sam).ExpectNoEvents().ExpectEvent(Add.OnGroup(1).WithNewItems(Sam).WithNewIndex(1))
                .AndExpectFinalCountOf(2);
        }

        [Test]
        public void GroupByIteratorAddSingleItemToNonExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorAddSingleItemToNonExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorAddMultipleItemsToExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorAddMultipleItemsToExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorAddMultipleItemsToNonExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorAddMultipleItemsToNonExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorAddMultipleItemsToMixedGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorAddMultipleItemsToMixedGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorAddMultipleItemsToMultipleNonExistingGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorAddMultipleItemsToMultipleNonExistingGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorRemoveSingleItemFromExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorRemoveSingleItemFromExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorRemoveSingleNonExisingItemFromNonExisingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorRemoveSingleNonExisingItemFromNonExisingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorRemoveMultipleItemsFromExistingGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorRemoveMultipleItemsFromExistingGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorRemoveMultipleItemsFromMixedGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorRemoveMultipleItemsFromMixedGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorMoveSingleItemInExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorMoveSingleItemInExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorMoveSingleNonExistingItemBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorMoveSingleNonExistingItemAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorMoveMultipleItemsToExistingGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorMoveMultipleItemsToExistingGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorMoveMultipleItemsToMixedGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorMoveMultipleItemsToMixedGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceSingleItemInExistingGroupWithItemForExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceSingleItemInExistingGroupWithItemForExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceSingleItemInExistingGroupWithItemForNonExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceSingleItemInExistingGroupWithItemForNonExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceSingleNonExistingItemInExistingGroupWithItemForNonExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceSingleNonExistingItemInExistingGroupWithItemForNonExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceSingleNonExistingItemInExistingGroupWithItemForExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceSingleNonExistingItemInExistingGroupWithItemForExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceSingleNonExistingItemInNonExistingGroupWithItemForExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceSingleNonExistingItemInNonExistingGroupWithItemForExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceSingleNonExistingItemInNonExistingGroupWithItemForNonExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceSingleNonExistingItemInNonExistingGroupWithItemForNonExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceMultipleExistingItemsForExistingGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceMultipleExistingItemsForExistingGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceMultipleExistingItemsForNonExistingGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceMultipleExistingItemsForNonExistingGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceMultipleNonExistingItemsForExistingGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceMultipleNonExistingItemsForExistingGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceMultipleNonExistingItemsForNonExistingGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceMultipleNonExistingItemsForNonExistingGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceMultipleMixedItemsForExistingGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceMultipleMixedItemsForExistingGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorReplaceMultipleMixedItemsForMixedGroupsBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorReplaceMultipleMixedItemsForMixedGroupsAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorChangeSingleItemKeyForExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorChangeSingleItemKeyForExistingGroupAfterInitialize()
        {

        }
        [Test]
        public void GroupByIteratorChangeSingleItemKeyForNonExistingGroupBeforeInitialize()
        {

        }

        [Test]
        public void GroupByIteratorChangeSingleItemKeyForNonExistingGroupAfterInitialize()
        {

        }
    }
}
