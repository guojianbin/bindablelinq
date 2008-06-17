using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Xml.Linq ;
using Bindable.Linq.Documentation.Transformation ;

namespace Bindable.Linq.Documentation
{
	public static class WikiGenerator
	{
		private static void Main( string[] args )
		{
			try
			{
				Arguments arguments = Arguments.ProcessArguments( args, new FileSystemService( ) ) ;
				Trace.TraceInformation( "Beginning documentation generation run at {0}".Inject( DateTime.Now.ToShortTimeString( ) ) ) ;
				var parser = new XmlDocParser( arguments.XmlDocs, arguments.TargetType ) {TargetAssembly = arguments.TargetAssembly} ;
				IEnumerable<XDocument> cleanedDocs = parser.Parse( ) ;
				var writer = new BatchWriter( cleanedDocs ) ;
				writer.Write( arguments.OutputDirectory ) ;
				Trace.TraceInformation( "Finished documentation generation run at {0}".Inject( DateTime.Now.ToShortTimeString( ) ) ) ;
			}
			catch ( ArgumentException e )
			{
				Console.WriteLine( e.Message ) ;
			}
			catch ( Exception e )
			{
				Trace.TraceInformation( "Documentation generation run failed at {0}".Inject( DateTime.Now.ToShortTimeString( ) ) ) ;
				Trace.TraceError( e.Message ) ;
				throw ;
			}
			//finally
			//{
			//    Console.ReadLine();
			//}
		}
	}
}