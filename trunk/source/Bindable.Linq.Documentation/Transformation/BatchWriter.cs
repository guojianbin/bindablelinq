using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Xml.Linq ;

namespace Bindable.Linq.Documentation.Transformation
{
	public class BatchWriter
	{
		private readonly IEnumerable<XDocument> _docs ;
		private IFileSystemService _fileSystem ;

		public BatchWriter( IEnumerable<XDocument> docs )
		{
			_docs = docs ;
		}

		public int Count { get ; private set ; }

		public IFileSystemService FileSystemService
		{
			get
			{
				if ( _fileSystem == null )
					_fileSystem = new FileSystemService( ) ;
				return _fileSystem ;
			}
			set { _fileSystem = value ; }
		}

		public void Write( string outputDir )
		{
			if ( String.IsNullOrEmpty( outputDir ) ) throw new ArgumentNullException( "outputDir" ) ;

			PrepareOutputDirectory( outputDir ) ;

			foreach ( XDocument doc in _docs )
			{
				XElement memberElement = doc.Element( "member" ) ;
				if ( memberElement == null )
					throw new InvalidOperationException( "Couldn't find member element." ) ;

				XAttribute nameAttr = memberElement.Attribute( "name" ) ;
				if ( nameAttr == null )
					throw new InvalidOperationException( "Couldn't find name attribute." ) ;

				string memberName = nameAttr.Value ;
				if ( String.IsNullOrEmpty( memberName ) )
					throw new InvalidOperationException( "Name attribute is empty." ) ;

				string filename = GetFilename( outputDir, memberName ) ;
				FileSystemService.CreateFileFrom( doc, filename ) ;

				Count++ ;
			}
		}


		internal string GetFilename( string outputDir, string memberName )
		{
			string filename ;
			int i = 1 ;
			bool firstRun = true ;
			do
			{
				filename = memberName ;

				if ( firstRun ) firstRun = false ;
				else
				{
					filename = "{0}{1}".Inject( memberName, i ) ;
					i++ ;
				}

				filename = Path.ChangeExtension( filename, "txt" ) ;
				filename = Path.Combine( outputDir, filename ) ;
			} while ( FileSystemService.DoesFileExist( filename ) ) ;
			return filename ;
		}


		internal void PrepareOutputDirectory( string outputDir )
		{
			if ( FileSystemService.DoesDirectoryExist( outputDir ) )
			{
				FileSystemService.RecursivelyDeleteDirectory( outputDir ) ;
			}

			FileSystemService.CreateDirectory( Path.GetFullPath( outputDir ) ) ;
		}
	}
}