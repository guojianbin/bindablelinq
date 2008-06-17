using System.Linq;
using Bindable.Linq.Documentation.Tests.Behaviour;
using Bindable.Linq.Documentation.Transformation;
using NUnit.Framework;

namespace Bindable.Linq.Documentation.Tests.Specifications
{
	[TestFixture]
	public class When_extracting_a_declaration : behaves_like_xmlDocumentation_parser
	{
		private const string str_simple_declaration = "public static string TestMethod(this string str)";

		private const string str_complexDeclaration =
			"public static System.Collections.Generic.IEnumerable<TSource> TestMethod<TSource>(this TSource source, System.Func<TSource, Func<String, Int32>> nestedGeneric) where TSource : System.IDisposable";

		[Test]
		public void Should_return_all_declarations()
		{
			var declarations = sut.GetDeclarationsFor("AsBindable");
			declarations.Count().MustEqual(5);
		}

		[Test]
		public void Should_return_correct_declaration_complex_declaration()
		{
			var type = typeof (TestClass);
			var methodInfo = type.GetMethods()[0];
			var decl = XmlDocParser.GetDeclarationFor(methodInfo);

			decl.MustEqual(str_complexDeclaration);
		}

		[Test]
		public void Should_return_correct_declaration_simple_declaration()
		{
			var type = typeof (TestClass);
			var methodInfo = type.GetMethods()[1];
			var decl = XmlDocParser.GetDeclarationFor(methodInfo);

			decl.MustEqual(str_simple_declaration);
		}
	}
}