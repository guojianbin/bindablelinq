using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Bindable.Linq.Documentation.Tests.Behaviour;
using NUnit.Framework;

namespace Bindable.Linq.Documentation.Tests.Specifications
{
	[TestFixture]
	public class When_parsing_xmlDocs : behaves_like_xmlDocumentation_parser
	{
		[Test]
		public void Should_clean_doc_comments()
		{
			var documents = sut.TransformDocComments(new List<XElement> {TestElement});
			foreach (var doc in documents)
			{
				// ReSharper disable PossibleNullReferenceException
				doc.Element(XName.Get("member"))
					.Attribute(XName.Get("name"))
					.Value.MustEqual("TestMethod");
				doc.Descendants(XName.Get("see")).First()
					.Attribute(XName.Get("cref"))
					.Value.MustEqual("Aggregator");
				// ReSharper restore PossibleNullReferenceException
			}
		}

		[Test]
		public void Should_create_at_least_one_declaration_element_for_each_method()
		{
			var documents = sut.Parse();
			foreach (var doc in documents)
				doc.MustContainElement("declaration");
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "XML Documentation not found.")]
		public void Should_throw_if_source_document_is_null()
		{
			sut.SourceDocument = null;
			sut.Parse();
		}
	}
}