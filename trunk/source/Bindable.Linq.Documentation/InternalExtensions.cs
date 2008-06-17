using System ;
using System.Globalization ;
using System.Text.RegularExpressions ;

namespace Bindable.Linq.Documentation
{
	internal static class InternalExtensions
	{
		internal static string Inject( this string format, params object[] p )
		{
			return String.Format( CultureInfo.CurrentCulture, format, p ) ;
		}

		internal static string RemoveMethodBody( this string input )
		{
			return input.Substring( 0, input.IndexOf( '{' ) ) ;
		}

		internal static string RemoveControlChars( this string input )
		{
			return input.Replace( "\r\n", string.Empty ) ;
		}

		internal static string NormalizeSpacing( this string input )
		{
			input = input.Replace( "  ", " " ) ;
			return input.Replace( "  ", " " ) ; //two times is the charm ;)
		}

		internal static string InsertExtensionMethodSyntax( this string input )
		{
			return Regex.Replace( input, @"\(", "(this " ) ;
		}
	}
}