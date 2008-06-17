using System ;
using System.IO ;
using System.Reflection ;
using System.Xml.Linq ;
using Bindable.Linq.Documentation.Transformation ;

namespace Bindable.Linq.Documentation
{
	internal class Arguments
	{
		private const string help_message =
			"usage: wikigen <dll_filename> <target_type_name> <output_directory>\nNOTE: contents of output dir will be recursively overwritten!" ;

		private readonly string _outputDir ;

		private readonly Assembly _targetAssembly ;
		private readonly string _targetType ;
		private readonly XDocument _xmlDocs ;

		private Arguments( Assembly assembly, XDocument docs, string type, string dir )
		{
			_targetAssembly = assembly ;
			_xmlDocs = docs ;
			_targetType = type ;
			_outputDir = dir ;
		}

		public Assembly TargetAssembly
		{
			get { return _targetAssembly ; }
		}

		public XDocument XmlDocs
		{
			get { return _xmlDocs ; }
		}

		public string TargetType
		{
			get { return _targetType ; }
		}

		public string OutputDirectory
		{
			get { return _outputDir ; }
		}

		internal static Arguments ProcessArguments( string[] args, IFileSystemService FileSystem )
		{
			Assembly targetAssembly = null ;
			string targetType = null ;
			XDocument xmlDocs = null ;
			string outputDir = null ;

			switch ( args.Length )
			{
				case 3:
					targetAssembly = FileSystem.GetAssemblyFrom( args[0] ) ;

					string xmlDocsFileName = Path.ChangeExtension( args[0], "xml" ) ;
					xmlDocs = FileSystem.GetXDocumentFrom( xmlDocsFileName ) ;

					targetType = args[1] ;
					outputDir = args[2] ;
					break ;
				default:
					ShowHelp( ) ;
					break ;
			}
			return new Arguments( targetAssembly, xmlDocs, targetType, outputDir ) ;
		}

		private static void ShowHelp( )
		{
			throw new ArgumentException( help_message ) ;
		}
	}
}