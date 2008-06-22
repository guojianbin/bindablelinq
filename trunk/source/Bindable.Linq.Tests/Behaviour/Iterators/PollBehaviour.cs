using System.Collections;
using System.Collections.Generic;
using Bindable.Linq.Tests.MockObjectModel;
using Bindable.Linq.Tests.TestLanguage.Helpers;
using NUnit.Framework;

namespace Bindable.Linq.Tests.Behaviour.Iterators
{
    /// <summary>
    /// Contains unit tests for the <see cref="T:PollIterator`1"/> class.
    /// </summary>
    [TestFixture]
    public class PollBehaviour : TestFixture
    {
        internal class NonBindableCollection : IEnumerable<Contact>
        {
            public int GetEnumeratorCalls;
            public List<Contact> Items = new List<Contact>();

            public NonBindableCollection(params Contact[] contacts)
            {
                Items.AddRange(contacts);
            }

            #region IEnumerable<Contact> Members
            public IEnumerator<Contact> GetEnumerator()
            {
                GetEnumeratorCalls++;
                return Items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion

            public void Add(Contact item)
            {
                Items.Add(item);
            }
        }
    }
}