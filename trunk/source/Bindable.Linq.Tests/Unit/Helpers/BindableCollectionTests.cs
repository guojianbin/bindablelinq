using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BindingOriented.SyncLinq.Dependencies;
using BindingOriented.SyncLinq.Iterators;
using BindingOriented.SyncLinq.Tests.TestObjectModel;
using BindingOriented.SyncLinq.Tests.TestHelpers;
using NUnit.Framework;
using BindingOriented.SyncLinq.Helpers;
using BindingOriented.SyncLinq.Collections;

namespace BindingOriented.SyncLinq.Tests.Unit.Helpers
{
    /// <summary>
    /// This class contains tests for the SyncLINQ BindableCollection utility class.
    /// </summary>
    [TestFixture]
    public sealed class BindableCollectionTests
    {
        /// <summary>
        /// Tests that when enumerating over a BindableCollection, items can be added to it.
        /// </summary>
        [Test]
        public void BindableCollectionEnumeratorFrozen()
        {
            // Initialize the test data
            BindableCollection<Contact> customers = new BindableCollection<Contact>();
            customers.Add(new Contact() { Name = "Paul" });
            customers.Add(new Contact() { Name = "Greg" });
            customers.Add(new Contact() { Name = "Sam" });

            // Enumerate over the items, and whilst enumerating, add some new items. The new items 
            // should be added and should not effect the items being enumerated.
            int enumerated = 0;
            foreach (Contact customer in customers)
            {
                enumerated++;
                // This would normally raise an InvalidOperationException
                customers.Add(new Contact() { Name = "Jack " + enumerated });
            }

            // Check that the items were actually added and enumerated correctly
            Assert.AreEqual(3, enumerated);
            Assert.AreEqual("Paul", customers[0].Name);
            Assert.AreEqual("Greg", customers[1].Name);
            Assert.AreEqual("Sam", customers[2].Name);
            Assert.AreEqual("Jack 1", customers[3].Name);
            Assert.AreEqual("Jack 2", customers[4].Name);
            Assert.AreEqual("Jack 3", customers[5].Name);
        }
    }
}
