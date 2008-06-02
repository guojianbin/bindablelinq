using System.ComponentModel;
using Bindable.Linq.Tests.TestHelpers;
using Bindable.Linq.Tests.TestObjectModel;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Unit.Adapters
{
    [TestFixture]
    public class BindingListAdapterTests : TestFixture
    {
        private IBindingList NewTestBindingList()
        {
            return Given.Collection(Brian, Gordon, Harry).OrderBy(c => c.Name).ToBindingList();
        }

        private static PropertyDescriptor NameProperty()
        {
            return TypeDescriptor.GetProperties(typeof (Contact))["Name"];
        }

        [Test]
        public void ContainsUnsorted()
        {
            var bindingList = NewTestBindingList();
            Assert.IsTrue(bindingList.Contains(Brian));
            Assert.IsTrue(bindingList.Contains(Gordon));
            Assert.IsTrue(bindingList.Contains(Harry));
            Assert.IsFalse(bindingList.Contains(Michael));
        }

        [Test]
        public void ContainsSortedDescendining()
        {
            var bindingList = NewTestBindingList();
            bindingList.ApplySort(NameProperty(), ListSortDirection.Descending);
            Assert.IsTrue(bindingList.Contains(Brian));
            Assert.IsTrue(bindingList.Contains(Gordon));
            Assert.IsTrue(bindingList.Contains(Harry));
            Assert.IsFalse(bindingList.Contains(Michael));
        }

        [Test][Ignore]
        public void FindUnsorted()
        {
            var bindingList = NewTestBindingList();
            Assert.AreEqual(0, bindingList.Find(NameProperty(), Brian.Name));
            Assert.AreEqual(1, bindingList.Find(NameProperty(), Gordon.Name));
            Assert.AreEqual(2, bindingList.Find(NameProperty(), Harry.Name));
            Assert.AreEqual(-1, bindingList.Find(NameProperty(), Michael.Name));
        }

    }

}