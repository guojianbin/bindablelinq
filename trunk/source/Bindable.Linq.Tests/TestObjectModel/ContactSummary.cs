using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bindable.Linq.Tests.TestObjectModel
{
    /// <summary>
    /// A summary of a contact, used for projection.
    /// </summary>
    public class ContactSummary : BindableObject
    {
        private string _summary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactSummary"/> class.
        /// </summary>
        public ContactSummary()
        {

        }

        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }
    }
}
