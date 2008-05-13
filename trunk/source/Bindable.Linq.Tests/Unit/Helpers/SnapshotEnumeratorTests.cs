//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Bindable.Linq.Dependencies;
//using NUnit.Framework;
//using Bindable.Linq.Helpers;

//namespace Bindable.Linq.Tests.Unit.Helpers
//{
//    /// <summary>
//    /// This class contains tests for the <see cref="T:SnapshotEnumerator"/> class.
//    /// </summary>
//    [TestFixture]
//    public class SnapshotEnumeratorTests
//    {
//        /// <summary>
//        /// Tests that the snapshot enumerator does indeed allow people to modify items while enumerating 
//        /// (a big no-no for most enumerators).
//        /// </summary>
//        [Test]
//        public void SnapshotEnumeratorCanModifyItemsWhileEnumerating()
//        {
//            // Create a test list to start us off - we need at least 2 items - one to enter the loop, and one 
//            // to show our modifications within the loop don't cause the enumerator to error.
//            BindableCollection<string> realItems = new BindableCollection<string>();
//            realItems.Add("Paul1");
//            realItems.Add("Paul2");

//            // Create the enumerator and enumerate the items
//            int enumerated = 0;
//            foreach (string item in realItems)
//            {
//                realItems.Add("ABC");       // A regular enumerator would have a fit
//                enumerated++;
//            }

//            // Ensure that we have the right number of items we expected (2 original, + 2 new)
//            Assert.AreEqual(4, realItems.Count);
//            Assert.AreEqual(2, enumerated);
//        }

//        /// <summary>
//        /// Tests that the snapshot enumerator does indeed allow people to modify items while enumerating 
//        /// (a big no-no for most enumerators).
//        /// </summary>
//        [Test]
//        public void SnapshotEnumeratorCanModifyItemsWhileEnumerating2()
//        {
//            // Create a test list to start us off - we need at least 2 items - one to enter the loop, and one 
//            // to show our modifications within the loop don't cause the enumerator to error.
//            List<string> realItems = new List<string>();
//            realItems.Add("Paul1");
//            realItems.Add("Paul2");

//            // Create the enumerator and enumerate the items
//            SnapshotEnumerator<string> enumerator = new SnapshotEnumerator<string>(realItems);
//            while (enumerator.MoveNext())
//            {
//                realItems.Add("ABC");       // A regular enumerator would have a fit
//                realItems.Add("ABC2");      // Add another, then we'll remove the one above
//                realItems.Remove("ABC");    // We can event remove
//                realItems.Add("Hello3");    // Add this one - we'll replace it with another
//                realItems[3] = "Goodbye3";  // ...and replace!

//                // Prove that the enumerator functions as a snapshot; even though other items have been 
//                // added (as tested further down in this test), the enumerator will not include them.
//                Assert.IsTrue(enumerator.Current == "Paul1" || enumerator.Current == "Paul2");
//            }

//            // Ensure that we have the right number of items we expected (2 original, + 2 new, +2 new again (second loop))
//            Assert.AreEqual(6, realItems.Count);
//            Assert.AreEqual(realItems[0], "Paul1");
//            Assert.AreEqual(realItems[1], "Paul2");
//            Assert.AreEqual(realItems[2], "ABC2");
//            Assert.AreEqual(realItems[3], "Goodbye3");
//            Assert.AreEqual(realItems[4], "ABC2");
//            Assert.AreEqual(realItems[5], "Hello3");
//        }

//        /// <summary>
//        /// Tests that the snapshot enumerator reset method works as expected.
//        /// </summary>
//        [Test]
//        public void SnapshotEnumeratorReset()
//        {
//            List<string> realItems = new List<string>();
//            realItems.Add("Paul1");
//            realItems.Add("Paul2");

//            IEnumerator enumerator = new SnapshotEnumerator<string>(realItems);
//            Assert.IsTrue(enumerator.MoveNext() && (string)enumerator.Current == "Paul1");
//            enumerator.Reset();
//            Assert.IsTrue(enumerator.MoveNext() && (string)enumerator.Current == "Paul1");
//            Assert.IsTrue(enumerator.MoveNext() && (string)enumerator.Current == "Paul2");
//            Assert.IsFalse(enumerator.MoveNext() && (string)enumerator.Current == null);
            
//        }
//    }
//}
