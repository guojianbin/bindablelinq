using System.Reflection;
using System.Xml.Linq;
using Bindable.Linq.Documentation.Transformation;

namespace Bindable.Linq.Documentation.Tests.Behaviour
{
	public class behaves_like_xmlDocumentation_parser : BehaviourBase
	{
		internal XmlDocParser sut;

		public XElement TestElement
		{
			get
			{
				var element =
					new XElement("member",
					             new XAttribute("name", "M:Bindable.Linq.Extensions.TestMethod`1"),
					             new XElement("summary",
					                          new XElement("see",
					                                       new XAttribute("cref", "T:Aggregator`2"),
					                                       "Aggregators"),
					                          "summary foo"),
					             new XElement("param",
					                          new XAttribute("name", "testParam"), "param foo"),
					             new XElement("returns",
					                          new XElement("see",
					                                       new XAttribute("cref", "T:Aggregator`2"), "returns foo")));

				return element;
			}
		}

		protected override void establish_context()
		{
			sut = new XmlDocParser(XDocument.Load(@"AssemblyUnderTest\Bindable.Linq.xml"), Constants.str_targetType)
			      {TargetAssembly = Assembly.LoadFrom(@"AssemblyUnderTest\Bindable.Linq.dll")};
		}
	}
}