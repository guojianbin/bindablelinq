using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using Bindable.Linq.Dependencies;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Tests.TestHelpers
{
    /// <summary>
    /// A base class for all Iterator tests fixtures.
    /// </summary>
    public abstract class TestFixture
    {
        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Add.
        /// </summary>
        protected NotifyCollectionChangedAction Add { get { return NotifyCollectionChangedAction.Add; } }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Remove.
        /// </summary>
        protected NotifyCollectionChangedAction Remove { get { return NotifyCollectionChangedAction.Remove; } }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Replace.
        /// </summary>
        protected NotifyCollectionChangedAction Replace { get { return NotifyCollectionChangedAction.Replace; } }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Move.
        /// </summary>
        protected NotifyCollectionChangedAction Move { get { return NotifyCollectionChangedAction.Move; } }

        /// <summary>
        /// Shortcut to NotifyCollectionChangedAction.Reset.
        /// </summary>
        protected NotifyCollectionChangedAction Reset { get { return NotifyCollectionChangedAction.Reset; } }

        protected Contact Mike = new Contact() { Name = "Mike" };
        protected Contact Sally = new Contact() { Name = "Sally" };
        protected Contact Sue = new Contact() { Name = "Sue" };
        protected Contact Lesley = new Contact() { Name = "Lesley" };
        protected Contact Tom = new Contact() { Name = "Tom" };
        protected Contact Jack = new Contact() { Name = "Jack" };
        protected Contact John = new Contact() { Name = "John" };
        protected Contact Mick = new Contact() { Name = "Mick" };
        protected Contact Simon = new Contact() { Name = "Simon" };
        protected Contact Clancy = new Contact() { Name = "Clancy" };
        protected Contact Brian = new Contact() { Name = "Brian" };
        protected Contact Harry = new Contact() { Name = "Harry" };
        protected Contact Rick = new Contact() { Name = "Rick" };
        protected Contact Paul = new Contact() { Name = "Paul" };
        protected Contact Phil = new Contact() { Name = "Phil" };
        protected Contact Ryan = new Contact() { Name = "Ryan" };
        protected Contact Tim = new Contact() { Name = "Tim" };
        protected Contact Jake = new Contact() { Name = "Jake" };
        protected Contact Bubsy = new Contact() { Name = "Bubsy" };
        protected Contact Gordon = new Contact() { Name = "Gordon" };
        protected Contact Jarryd = new Contact() { Name = "Jarryd" };
        protected Contact Michelle = new Contact() { Name = "Michelle" };
        protected Contact Michael = new Contact() { Name = "Michael" };
        protected Contact Sam = new Contact() { Name = "Sam" };
        
        /// <summary>
        /// Compares a Bindable LINQ query with a LINQ query.
        /// </summary>
        /// <param name="syncLinqCollection">The sync linq collection.</param>
        /// <param name="linqQuery">The linq query.</param>
        protected void CompareWithLinqOrdered(IBindableQuery syncLinqCollection, IEnumerable linqQuery)
        {
            CompatibilityValidator.CompareWithLinq(CompatibilityExpectation.FullyCompatible, syncLinqCollection, linqQuery);
        }

        /// <summary>
        /// Compares a Bindable LINQ query with a LINQ query.
        /// </summary>
        /// <param name="syncLinqCollection">The sync linq collection.</param>
        /// <param name="linqQuery">The linq query.</param>
        protected void CompareWithLinqUnordered(IBindableQuery syncLinqCollection, IEnumerable linqQuery)
        {
            CompatibilityValidator.CompareWithLinq(CompatibilityExpectation.FullyCompatibleExceptOrdering, syncLinqCollection, linqQuery);
        }
    }
}
