using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Bindable.Linq.Collections;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Helpers;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// This class contains tests for the Bindable LINQ UnionIterator class.
    /// </summary>
    [TestFixture]
    public class UnionBehaviour : TestFixture
    {
        #region Test Equality Comparer 

        internal class ContactEqualityComparer : IEqualityComparer<Contact>
        {
            public bool Equals(Contact x, Contact y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(Contact obj)
            {
                return obj.GetHashCode();
            }
        }

        #endregion

        /// <summary>
        /// Tests that a simple union works.
        /// </summary>
        [Test]
        public void UnionIteratorSimpleUnion()
        {
            // Setup the test data
            BindableCollection<Contact> inputs = new BindableCollection<Contact>();
            ObservableCollection<Contact> customerSource1 = new ObservableCollection<Contact>();
            customerSource1.Add(new Contact() { Name = "Paul Stovell" });
            customerSource1.Add(new Contact() { Name = "Paul Glavich" });
            ObservableCollection<Contact> customerSource2 = new ObservableCollection<Contact>();
            customerSource2.Add(new Contact() { Name = "Jennifer Lopez" });
            customerSource2.Add(new Contact() { Name = "Fedde Le Grande" });
            
            // Create the query
            var result = customerSource1
                         .AsBindable()
                         .Union(
                            customerSource2
                            .AsBindable()
                         );
            CollectionEventCatcher eventCatcher = new CollectionEventCatcher(result);
            
            // Check the results. No collection changed events should have been raised (the items were 
            // read in the enumerator). 
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(0, eventCatcher.Count);
            Assert.AreEqual("Paul Stovell", result[0].Name);
            Assert.AreEqual("Paul Glavich", result[1].Name);
            Assert.AreEqual("Jennifer Lopez", result[2].Name);
            Assert.AreEqual("Fedde Le Grande", result[3].Name);
            CompareWithLinqOrdered(
                result,
                customerSource1.Union(customerSource2)
                );
        }
    }
}
