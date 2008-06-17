using System;
using System.Reflection;
using Bindable.Linq.Documentation.Tests.Behaviour;
using NUnit.Framework;

namespace Bindable.Linq.Documentation.Tests.Specifications
{
	[TestFixture]
	public class When_processing_command_line_args : behaves_like_argument_object_with_fileSystem_in_play
	{
		[Test]
		public void Should_return_outputDir()
		{
			sut.OutputDirectory.MustEqual(OutputDir);
		}

		[Test]
		public void Should_return_target_assembly()
		{
			sut.TargetAssembly.MustEqual(Assembly.GetExecutingAssembly());
		}

		[Test]
		public void Should_return_targetType()
		{
			sut.TargetType.MustEqual(TargetTypeName);
		}

		[Test]
		public void Should_return_xmlDocs()
		{
			sut.XmlDocs.MustEqual(ExpectedXDocument);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void Should_throw_if_no_parameters_are_passed()
		{
			var badArgs = new string[] {};
			Arguments.ProcessArguments(badArgs, FileSystem);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void Should_throw_if_one_parameter_is_passed()
		{
			var badArgs = new[] {"kthxbye"};
			Arguments.ProcessArguments(badArgs, FileSystem);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void Should_throw_if_two_parameters_are_passed()
		{
			var badArgs = new[] {"rofl", "lmao"};
			Arguments.ProcessArguments(badArgs, FileSystem);
		}
	}
}