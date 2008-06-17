using System;
using System.Collections.Generic;

namespace Bindable.Linq.Documentation.Tests.Specifications
{
	internal static class TestClass
	{
		public static IEnumerable<TSource> TestMethod<TSource>(this TSource source,
		                                                       Func<TSource, Func<string, int>> nestedGeneric)
			where TSource : IDisposable
		{
			throw new NotImplementedException();
		}

		public static string TestMethod(this string str)
		{
			throw new NotImplementedException();
		}
	}
}