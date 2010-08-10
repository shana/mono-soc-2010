using System;
using System.IO;
using System.CodeDom;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mono.VisualC.Code.Atoms {

	public class Union : CodeContainer {

		public string Name { get; set; }

		public Union (string name)
		{
			Name = name;
		}

		public CodeTypeDeclaration CreateUnionType ()
		{
			var union = new CodeTypeDeclaration (Name) {
				Attributes = MemberAttributes.Public,
				TypeAttributes = TypeAttributes.Public | TypeAttributes.ExplicitLayout,
				IsStruct = true
			};

			foreach (var atom in Atoms) {
				Field field = atom as Field;
				if (field == null)
					throw new Exception ("Only Fields allowed in Union.");

				CodeMemberField cmf = field.InsideCodeTypeDeclaration (union) as CodeMemberField;
				if (cmf != null)
					cmf.CustomAttributes.Add (new CodeAttributeDeclaration (new CodeTypeReference (typeof (FieldOffsetAttribute)), new CodeAttributeArgument (new CodePrimitiveExpression (0))));
			}

			return union;
		}

		internal protected override object InsideCodeNamespace (CodeNamespace ns)
		{
			ns.Types.Add (CreateUnionType ());
			return null;
		}

		internal protected override object InsideCodeTypeDeclaration (CodeTypeDeclaration decl)
		{
			decl.Members.Add (CreateUnionType ());
			return null;
		}

		public override void Write (TextWriter writer)
		{
			writer.WriteLine ("union {0} {{", Name);
			base.Write (writer);
			writer.WriteLine ("}");
		}
	}
}

