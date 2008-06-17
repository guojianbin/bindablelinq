using System.Diagnostics ;
using System.IO ;
using System.Reflection ;
using System.Xml ;
using System.Xml.Linq ;
using System.Xml.Xsl ;

namespace Bindable.Linq.Documentation.Transformation
{
	internal class FileSystemService : IFileSystemService
	{
		private XslCompiledTransform _xslt ;

		internal XslCompiledTransform Stylesheet
		{
			get
			{
				if ( _xslt == null )
					_xslt = GetStylesheet( ) ;
				return _xslt ;
			}
		}

		#region IFileSystemService Members

		public bool DoesDirectoryExist( string directory )
		{
			return Directory.Exists( directory ) ;
		}

		public void CreateDirectory( string directory )
		{
			Directory.CreateDirectory( directory ) ;
		}

		public void RecursivelyDeleteDirectory( string directory )
		{
			Trace.TraceInformation( "Removing directory {0}".Inject( directory ) ) ;
			Directory.Delete( directory, true ) ;
		}

		public bool DoesFileExist( string filename )
		{
			return File.Exists( filename ) ;
		}

		public void CreateFileFrom( XDocument doc, string filename )
		{
			using ( StreamWriter file = File.CreateText( filename ) )
			{
				Trace.TraceInformation( "Writing to {0}.".Inject( filename ) ) ;
				WriteToFile( doc, file ) ;
			}
		}

		public Assembly GetAssemblyFrom( string path )
		{
			return Assembly.LoadFrom( path ) ;
		}

		public XDocument GetXDocumentFrom( string fileName )
		{
			return XDocument.Load( fileName ) ;
		}

		#endregion

		private static XslCompiledTransform GetStylesheet( )
		{
			var xslt = new XslCompiledTransform( true ) ;
			string xsltPath = @".\member.xslt" ; //todo: get this from user config
			Trace.TraceInformation( "Loading XSLT" ) ;
			xslt.Load( xsltPath ) ;
			return xslt ;
		}

		private void WriteToFile( XNode doc, StreamWriter file )
		{
			try
			{
				XmlReader xr = doc.CreateReader( ) ;
				Stylesheet.Transform( xr, null, file.BaseStream ) ;
			}
			catch ( XsltException e )
			{
				Trace.TraceError( e.Message ) ;
				throw ;
			}
		}
	}
}