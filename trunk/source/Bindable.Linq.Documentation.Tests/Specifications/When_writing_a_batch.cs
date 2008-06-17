using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Bindable.Linq.Documentation.Tests.Behaviour;
using Bindable.Linq.Documentation.Transformation;
using NUnit.Framework;
using Rhino.Mocks;

namespace Bindable.Linq.Documentation.Tests.Specifications
{
	[TestFixture]
	public class When_writing_a_batch : behaves_like_batch_writer_with_fileSystem_in_play
	{
		[Test]
		public void Should_change_filenames_until_filename_doesnt_exist()
		{
			var expectedPath = Path.Combine(OutputDirectory, "foobar1.txt");
			using (Record)
			{
				Expect
					.Call(FileSystem.DoesFileExist(Path.Combine(OutputDirectory, "foobar.txt")))
					.Return(true);

				Expect
					.Call(FileSystem.DoesFileExist(expectedPath))
					.Return(false);
			}
			using (PlayBack)
			{
				sut.GetFilename(OutputDirectory, "foobar").MustEqual(expectedPath);
			}
		}

		[Test]
		public void Should_create_one_file_per_xdocument()
		{
			using (Record)
			{
				Expect
					.Call(() =>
					      FileSystem.CreateFileFrom(new XDocument(), "foo"))
					.IgnoreArguments()
					.Repeat.Times(DocumentCount);
			}
			using (PlayBack)
				sut.Write(OutputDirectory);
		}

		[Test]
		public void Should_create_output_folder_if_it_doesnt_exist()
		{
			using (Record)
			{
				Expect
					.Call(FileSystem.DoesDirectoryExist(OutputDirectory))
					.Return(false);

				FileSystem.CreateDirectory(Path.GetFullPath(OutputDirectory));
			}
			using (PlayBack)
				sut.PrepareOutputDirectory(OutputDirectory);
		}

		[Test]
		public void Should_delete_existing_folder_contents()
		{
			using (Record)
			{
				Expect
					.Call(FileSystem.DoesDirectoryExist(OutputDirectory))
					.Return(true);

				FileSystem.RecursivelyDeleteDirectory(OutputDirectory);
			}
			using (PlayBack)
				sut.PrepareOutputDirectory(OutputDirectory);
		}

		[Test]
		public void Should_instantiate_a_file_system_service_when_required()
		{
			sut.FileSystemService = null;
			sut.FileSystemService.MustBeInstantiated();
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void Should_throw_if_outputDir_is_empty()
		{
			sut.Write("");
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void Should_throw_if_outputDir_is_null()
		{
			sut.Write(null);
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "Couldn't find member element.")]
		public void Should_throw_when_member_element_is_missing()
		{
			var docs = new List<XDocument> {new XDocument()};
			sut = new BatchWriter(docs) {FileSystemService = FileSystem};

			using (Record)
			{
				Expect
					.Call(FileSystem.DoesDirectoryExist(OutputDirectory))
					.Return(false);
			}
			using (PlayBack)
			{
				sut.Write(OutputDirectory);
			}
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "Name attribute is empty.")]
		public void Should_throw_when_name_attribute_is_empty()
		{
			var docs = new List<XDocument> {new XDocument(new XElement("member", new XAttribute("name", "")))};
			sut = new BatchWriter(docs) {FileSystemService = FileSystem};
			using (Record)
			{
				Expect
					.Call(FileSystem.DoesDirectoryExist(OutputDirectory))
					.Return(false);
			}
			using (PlayBack)
			{
				sut.Write(OutputDirectory);
			}
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException), ExpectedMessage = "Couldn't find name attribute.")]
		public void Should_throw_when_name_attribute_is_missing()
		{
			var docs = new List<XDocument> {new XDocument(new XElement("member"))};
			sut = new BatchWriter(docs) {FileSystemService = FileSystem};
			using (Record)
			{
				Expect
					.Call(FileSystem.DoesDirectoryExist(OutputDirectory))
					.Return(false);
			}
			using (PlayBack)
			{
				sut.Write(OutputDirectory);
			}
		}
	}
}