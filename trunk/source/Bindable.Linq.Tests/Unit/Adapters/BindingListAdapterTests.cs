using System.ComponentModel;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Unit.Adapters
{
    [TestFixture]
    public class BindingListAdapterTests : TestFixture
    {
        #region Test Helpers
        private IBindingList NewTestBindingList()
        {
            return With.Inputs(Gordon, Brian, Harry).OrderBy(c => c.Name).ToBindingList();
        }

        private IBindingList NewTestBindingListSortedDescending()
        {
            var bindingList = NewTestBindingList();
            bindingList.ApplySort(NameProperty(), ListSortDirection.Descending);
            return bindingList;
        }

        private static PropertyDescriptor NameProperty()
        {
            return TypeDescriptor.GetProperties(typeof (Contact))["Name"];
        }
        #endregion

        [Test]
        public void ContainsNaturalOrder()
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
            var bindingList = NewTestBindingListSortedDescending();
            Assert.IsTrue(bindingList.Contains(Brian));
            Assert.IsTrue(bindingList.Contains(Gordon));
            Assert.IsTrue(bindingList.Contains(Harry));
            Assert.IsFalse(bindingList.Contains(Michael));
        }

        [Test]
        public void FindNaturalOrder()
        {
            var bindingList = NewTestBindingList();
            Assert.AreEqual(0, bindingList.Find(NameProperty(), Brian.Name));
            Assert.AreEqual(1, bindingList.Find(NameProperty(), Gordon.Name));
            Assert.AreEqual(2, bindingList.Find(NameProperty(), Harry.Name));
            Assert.AreEqual(-1, bindingList.Find(NameProperty(), Michael.Name));
        }

        [Test]
        public void FindSortedDescending()
        {
            var bindingList = NewTestBindingListSortedDescending();
            Assert.AreEqual(2, bindingList.Find(NameProperty(), Brian.Name));
            Assert.AreEqual(1, bindingList.Find(NameProperty(), Gordon.Name));
            Assert.AreEqual(0, bindingList.Find(NameProperty(), Harry.Name));
            Assert.AreEqual(-1, bindingList.Find(NameProperty(), Michael.Name));
        }

        [Test]
        public void FindSortedDescendingThenRemoveSort()
        {
            var bindingList = NewTestBindingListSortedDescending();
            bindingList.RemoveSort();
            Assert.AreEqual(0, bindingList.Find(NameProperty(), Brian.Name));
            Assert.AreEqual(1, bindingList.Find(NameProperty(), Gordon.Name));
            Assert.AreEqual(2, bindingList.Find(NameProperty(), Harry.Name));
            Assert.AreEqual(-1, bindingList.Find(NameProperty(), Michael.Name));
        }

        [Test]
        public void IndexerNaturalOrder()
        {
            var bindingList = With.Inputs(Brian, Gordon, Harry).AsBindable().ToBindingList();
            Assert.AreSame(Brian, bindingList[0]);
            Assert.AreSame(Gordon, bindingList[1]);
            Assert.AreSame(Harry, bindingList[2]);
        }
    }
}
