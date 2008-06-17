using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Bindable.Linq.Documentation.Transformation;
using Rhino.Mocks;

namespace Bindable.Linq.Documentation.Tests.Behaviour
{
	public class behaves_like_argument_object_with_fileSystem_in_play : BehaviourBase
	{
		internal XDocument ExpectedXDocument = new XDocument();
		internal IFileSystemService FileSystem;
		internal string OutputDir = @".\docs";
		internal Arguments sut;
		internal string TargetDLLPath = @".\foo.dll";
		internal string TargetTypeName = "Foo.Bar";

		protected override void establish_context()
		{
			var args = new[] {TargetDLLPath, TargetTypeName, OutputDir};
			FileSystem = Dependancy<IFileSystemService>();
			using (Record)
			{
				Expect
					.Call(FileSystem.GetAssemblyFrom(TargetDLLPath))
					.Return(Assembly.GetExecutingAssembly());
				Expect
					.Call(FileSystem.GetXDocumentFrom(Path.ChangeExtension(TargetDLLPath, "xml")))
					.Return(ExpectedXDocument);
			}
			using (PlayBack)
			{
				sut = Arguments.ProcessArguments(args, FileSystem);
			}
		}
	}
}