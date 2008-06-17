using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.CSharp;

namespace Bindable.Linq.Documentation.Transformation
{
	public class XmlDocParser
	{
		private Type _targetType;

		public XmlDocParser(XDocument document, string targetTypeName)
		{
			SourceDocument = document;
			TargetTypeName = targetTypeName;
		}

		private string TargetTypeName { get; set; }
		public XDocument SourceDocument { get; set; }
		public Assembly TargetAssembly { get; set; }

		public Type TargetType
		{
			get
			{
				if (_targetType == null)
					_targetType = TargetAssembly.GetType(TargetTypeName, true);
				return _targetType;
			}
		}

		public IEnumerable<XDocument> Parse()
		{
			if (SourceDocument == null) throw new InvalidOperationException("XML Documentation not found.");
			//choose only those member elements to do with the TargetTypeName
			var members = from m in SourceDocument.Descendants("member")
			              let name = m.Attribute(XName.Get("name"))
			              where name != null && name.Value.Contains("M:" + TargetTypeName)
			              select m;

			return TransformDocComments(members);
		}

		internal IEnumerable<XDocument> TransformDocComments(IEnumerable<XElement> members)
		{
			foreach (var m in members)
			{
				var nameAttr = m.Attribute("name");

				if (nameAttr != null)
				{
					var qName = nameAttr.Value;
					Trace.TraceInformation("Cleaning member element for {0}.".Inject(qName));

					nameAttr.Value = GetTypeNameFromQName(qName);
					m.Add(GetDeclarationsFor(nameAttr.Value)); //add the declaration elements
				}

				var descendants = m.Descendants();

				foreach (var r in descendants)
				{
					//cleanup any cref attributes
					var crefAttr = r.Attribute(XName.Get("cref"));
					if (crefAttr == null) continue;
					var qName = crefAttr.Value;
					crefAttr.Value = GetTypeNameFromQName(qName);
				}

				var doc = new XDocument(m);

				yield return doc;
			}
		}

		internal static string GetTypeNameFromQName(string inputString)
		{
			var outputString = inputString.Split(':')[1];
			outputString = outputString.Split(new[] {'`', '('})[0];
			outputString = outputString.Split('.').Last();
			return outputString;
		}

		internal IEnumerable<XElement> GetDeclarationsFor(string methodName)
		{
			var methodInfos = from m in TargetType.GetMethods()
			                  where m.Name == methodName
			                  select m;

			var declarationsFor = new List<XElement>();
			if (methodInfos.Count() == 0)
				Trace.TraceWarning("Couldn't find methodInfo for {0}".Inject(methodName));

			foreach (var m in methodInfos)
			{
				var decl = GetDeclarationFor(m);
				var newElement =
					new XElement("declaration", decl);
				declarationsFor.Add(newElement);
			}

			Trace.TraceInformation("Retrieving declarations for {0}".Inject(methodName));
			return declarationsFor;
		}

		internal static string GetDeclarationFor(MethodInfo method)
		{
			var cmm = new CodeMemberMethod {
			                               	Name = method.Name,
			                               	ReturnType = GetTypeDeclarationFrom(method.ReturnType),
			                               	Attributes = GetMemberAttributesFrom(method)
			                               };

			var typeParameters = GetTypeParametersFrom(method);
			if (typeParameters.Any())
				cmm.TypeParameters.AddRange(typeParameters.ToArray());

			var parameters = GetParametersFrom(method);
			if (parameters.Any())
				cmm.Parameters.AddRange(parameters.ToArray());

			return GenerateCode(cmm);
		}

		private static string GenerateCode(CodeTypeMember cmm)
		{
			StringBuilder sb;
			using (var csp = new CSharpCodeProvider())
			{
				sb = new StringBuilder();
				using (var sw = new StringWriter(sb, CultureInfo.CurrentCulture))
				{
					var options = new CodeGeneratorOptions();
					csp.GenerateCodeFromMember(cmm, sw, options);
				}
			}
			var decl = sb.ToString().Trim();
			decl = CleanupDeclarationString(decl);

			return decl;
		}

		private static IEnumerable<CodeParameterDeclarationExpression> GetParametersFrom(MethodBase method)
		{
			foreach (var param in method.GetParameters())
			{
				var paramExpression = new CodeParameterDeclarationExpression(GetTypeDeclarationFrom(param.ParameterType),
				                                                             param.Name);
				if (param.ParameterType.IsGenericParameter)
				{
					paramExpression.Type = new CodeTypeReference(param.ParameterType.Name);
				}

				yield return paramExpression;
			}
		}

		private static IEnumerable<CodeTypeParameter> GetTypeParametersFrom(MethodBase method)
		{
			foreach (var typeParam in method.GetGenericArguments())
			{
				var ctp = new CodeTypeParameter(typeParam.Name);
				var constraints = typeParam.GetGenericParameterConstraints();

				if (constraints.Any())
				{
					foreach (var constraint in constraints)
						ctp.Constraints.Add(new CodeTypeReference(constraint));
				}
				yield return ctp;
			}
		}

		private static MemberAttributes GetMemberAttributesFrom(MethodBase method)
		{
			var flags = MemberAttributes.Public;

			if (method.IsStatic) flags |= MemberAttributes.Static;
			if (!method.IsVirtual) flags |= MemberAttributes.Final;

			return flags;
		}

		private static CodeTypeReference GetTypeDeclarationFrom(Type type)
		{
			var typeDef = type;
			if (typeDef.IsGenericType)
			{
				typeDef = typeDef.GetGenericTypeDefinition();
			}
			var reference = new CodeTypeReference(typeDef, CodeTypeReferenceOptions.GenericTypeParameter);

			FillTypeArguments(type, reference);

			return reference;
		}

		/// <remarks>Recursive</remarks>
		private static void FillTypeArguments(Type type, CodeTypeReference reference)
		{
			if (!type.IsGenericType) return;
			foreach (var typeParam in type.GetGenericArguments())
			{
				var newReference = new CodeTypeReference(typeParam.Name);
				FillTypeArguments(typeParam, newReference);
				reference.TypeArguments.Add(newReference);
			}
		}

		private static string CleanupDeclarationString(string decl)
		{
			var output = decl.InsertExtensionMethodSyntax();
			output = output.RemoveMethodBody();
			output = output.RemoveControlChars();
			output = output.Trim();
			output = output.NormalizeSpacing();
			return output;
		}
	}
}