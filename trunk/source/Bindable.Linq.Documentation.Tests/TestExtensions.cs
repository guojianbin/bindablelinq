using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace Bindable.Linq.Documentation.Tests
{
	internal static class TestExtensions
	{
		/// <summary>
		/// Asserts that the value specified by expected is equal to the actual value.
		/// </summary>
		internal static void MustEqual<T>(this T actual, T expected)
		{
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Asserts that the specified element is contained within the actual document.
		/// </summary>
		internal static void MustContainElement(this XDocument actual, string elementName)
		{
			var element = actual.Descendants(XName.Get(elementName));
			Assert.Greater(element.Count(), 0);
		}

		internal static void MustNotContainElement(this XDocument actual, string elementName)
		{
			var element = actual.Descendants(XName.Get(elementName));
			element.Count().MustEqual(0);
		}

		internal static void MustBeInstantiated<T>(this T actual)
		{
			Assert.IsInstanceOfType(typeof (T), actual);
		}

		internal static void MustBeReferentiallyEqualTo<T>(this T actual, T expected)
		{
			Assert.AreSame(expected, actual);
		}
	}
}