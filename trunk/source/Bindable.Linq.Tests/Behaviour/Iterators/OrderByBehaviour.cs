using System;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    using Collections;
    using NUnit.Framework;
    using TestHelpers;
    using TestObjectModel;

    /// <summary>
    /// Contains unit tests for the <see cref="T:OrderByIterator`1"/> class.
    /// </summary>
    [TestFixture]
    public class OrderByBehaviour : TestFixture
    {
        /// <summary>
        /// Tests that a ThenBy order query works along with an OrderBy clause.
        /// </summary>
        [Test]
        public void OrderByIteratorOrderByAscendingThenByAscending()
        {
            // Initialize the test data
            var inputs = new BindableCollection<Contact>();
            inputs.Add(new Contact {Name = "Paul Glavich", Company = "Readify"});
            inputs.Add(new Contact {Name = "Paul Stovell", Company = "Readify"});
            inputs.Add(new Contact {Name = "Paul Stovell", Company = "Raedify"});

            // Create the query
            IOrderedBindableQuery<Contact> result = inputs.AsBindable().OrderBy(c => c.Name).ThenBy(c => c.Company);
            var eventCatcher = new CollectionEventCatcher(result);

            // Check that the results are in the correct order 
            Assert.AreEqual(0, eventCatcher.Count);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Readify", result[0].Company);
            Assert.AreEqual("Raedify", result[1].Company);
            Assert.AreEqual("Readify", result[2].Company);
            CompareWithLinqOrdered(result, inputs.OrderBy(c => c.Name).ThenBy(c => c.Company));

            // Now add an item and ensure it is sorted correctly. An "Add" CollectionChanged event should 
            // be raised with the correct index and the item should have been inserted into the correct place.
            inputs.Add(new Contact {Name = "Paul Stovell", Company = "Fake"});
            Assert.AreEqual(1, eventCatcher.Count);
            Assert.AreEqual(Add, eventCatcher[0].Action);
            Assert.AreEqual(1, eventCatcher[0].NewStartingIndex);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Readify", result[0].Company);
            Assert.AreEqual("Fake", result[1].Company);
            Assert.AreEqual("Raedify", result[2].Company);
            Assert.AreEqual("Readify", result[3].Company);
            CompareWithLinqOrdered(result, inputs.OrderBy(c => c.Name).ThenBy(c => c.Company));
        }

        /// <summary>
        /// Tests that a simple OrderBy ascending query works.
        /// </summary>
        [Test]
        public void OrderByIteratorSingleAscendingSort()
        {
            // Initialize the test data
            var inputs = new BindableCollection<Contact>();
            inputs.Add(new Contact {Name = "Zak Azeez"});
            inputs.Add(new Contact {Name = "Aaron Saikovski"});
            inputs.Add(new Contact {Name = "Mitch Denny"});

            // Create the query
            IOrderedBindableQuery<Contact> result = inputs.AsBindable().OrderBy(c => c.Name);
            var eventCatcher = new CollectionEventCatcher(result);

            // Ensure the items are all there and in the correct order.
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Aaron Saikovski", result[0].Name);
            Assert.AreEqual("Mitch Denny", result[1].Name);
            Assert.AreEqual("Zak Azeez", result[2].Name);
            CompareWithLinqOrdered(result, inputs.OrderBy(c => c.Name));

            // Now add an item. It should raise an "Add" CollectionChanged event with the correct index 
            // (1 in this case, as Bill fits between Aaron and Mitch) and the list should reflect this.
            inputs.Add(new Contact {Name = "Bill Chesnut"});
            Assert.AreEqual(1, eventCatcher.Count);
            Assert.AreEqual(Add, eventCatcher[0].Action);
            Assert.AreEqual(1, eventCatcher[0].NewStartingIndex);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("Aaron Saikovski", result[0].Name);
            Assert.AreEqual("Bill Chesnut", result[1].Name);
            Assert.AreEqual("Mitch Denny", result[2].Name);
            Assert.AreEqual("Zak Azeez", result[3].Name);
            CompareWithLinqOrdered(result, inputs.OrderBy(c => c.Name));
        }
    }
}