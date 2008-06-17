using System.Collections.Generic;
using System.Xml.Linq;
using Bindable.Linq.Documentation.Transformation;

namespace Bindable.Linq.Documentation.Tests.Behaviour
{
	public class behaves_like_batch_writer_with_fileSystem_in_play : BehaviourBase
	{
		internal static int DocumentCount;
		internal static IFileSystemService FileSystem;
		internal static string OutputDirectory;
		internal BatchWriter sut;

		protected override void establish_context()
		{
			var docs = GetListOfXDocuments();

			FileSystem = Dependancy<IFileSystemService>();
			OutputDirectory = @".\docs\";

			sut = new BatchWriter(docs) {FileSystemService = FileSystem};
		}

		private static List<XDocument> GetListOfXDocuments()
		{
			var docs = new List<XDocument>();
			var doc1 = new XDocument(new XElement("member", new XAttribute("name", "foomethod")));
			var doc2 = new XDocument(new XElement("member", new XAttribute("name", "barmethod")));
			docs.Add(doc1);
			docs.Add(doc2);
			DocumentCount = docs.Count;
			return docs;
		}
	}
}