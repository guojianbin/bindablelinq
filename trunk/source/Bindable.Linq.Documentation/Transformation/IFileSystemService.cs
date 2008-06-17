using System.Reflection ;
using System.Xml.Linq ;

namespace Bindable.Linq.Documentation.Transformation
{
	public interface IFileSystemService
	{
		bool DoesDirectoryExist( string directory ) ;
		void CreateDirectory( string directory ) ;
		void RecursivelyDeleteDirectory( string directory ) ;
		bool DoesFileExist( string fileName ) ;
		void CreateFileFrom( XDocument doc, string fileName ) ;
		Assembly GetAssemblyFrom( string path ) ;
		XDocument GetXDocumentFrom( string fileName ) ;
	}
}