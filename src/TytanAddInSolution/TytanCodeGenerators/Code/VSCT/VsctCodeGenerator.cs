using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Pretorianie.Tytan.Core.BaseGenerators;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Code.VSCT
{
    /// <summary>
    /// This is the generator class. 
    /// When setting the 'Custom Tool' property of a C#, VB, or J# project item to "VsctGenerator", 
    /// the GenerateCode function will get called and will return the contents of the generated file 
    /// to the project system
    /// </summary>
    [ComVisible(true)]
    [Guid("2E1B4D86-9CA5-490d-8D2B-FC8F6AF56EB2")]
    [CodeGeneratorRegistration(typeof(VsctCodeGenerator), "VsctGenerator",
        Description = "Generates .NET source code for given VS IDE GUI definitions.",
        GeneratesCSharpSourceCode = true, GeneratesVBasicSouceCode = true, GeneratesDesignTimeSource = true)]
    public class VsctCodeGenerator : BaseCodeGeneratorWithSite
    {
        protected override string GenerateStringCode(string inputFileContent)
        {
            string globalNamespaceName;
            string guidListClassName;
            string cmdIdListClassName;
            string supporterPostfix;
            bool isPublic;

            // get parameters passed as 'FileNamespace' inside properties of the file generator:
            InterpreteArguments((string.IsNullOrEmpty(FileNamespace) ? null : FileNamespace.Split(';')),
                                out globalNamespaceName, out guidListClassName, out cmdIdListClassName,
                                out supporterPostfix, out isPublic);

            // create support CodeDOM classes:
            CodeNamespace globalNamespace = new CodeNamespace(globalNamespaceName);
            CodeTypeDeclaration classGuideList = CreateClass(guidListClassName, VsctComments.ClassGuideListComment, isPublic);
            CodeTypeDeclaration classPkgCmdIDList = CreateClass(cmdIdListClassName, VsctComments.ClassPkgCmdIDListComments, isPublic);
            CodeModelLanguages currentLanguage = CodeHelper.GetCodeLanguage(GetProject().CodeModel.Language);
            IList<NamedValue> guids;
            IList<NamedValue> ids;

            // retrieve the list GUIDs and IDs defined inside VSCT file:
            VsctParser.Parse(inputFileContent, out guids, out ids);

            // generate members describing GUIDs:
            if (guids != null)
            {
                foreach (NamedValue s in guids)
                {
                    s.Supporter = s.Name + supporterPostfix;
                    classGuideList.Members.Add(CreateConstField("System.String", s.Supporter, s.Value, true));
                }

                foreach (NamedValue g in guids)
                    classGuideList.Members.Add(CreateStaticField("Guid", g.Name, g.Supporter, true));
            }

            // generate members describing IDs:
            if (ids != null)
            {
                foreach (NamedValue i in ids)
                    classPkgCmdIDList.Members.Add(CreateConstField("System.UInt32", i.Name,
                                                                   ConversionHelper.ToHex(i.Value, currentLanguage),
                                                                   false));
            }

            // add all members to final namespace:
            globalNamespace.Imports.Add(new CodeNamespaceImport("System"));
            globalNamespace.Types.Add(classGuideList);
            globalNamespace.Types.Add(classPkgCmdIDList);

            // generate source code:
            return CodeHelper.GenerateFromNamespace(GetCodeProvider(), globalNamespace, true);
        }

        private void InterpreteArguments(string[] args, out string globalNamespaceName, out string guidClassName, out string cmdIdListClassName, out string supporterPostfix, out bool isPublic)
        {
            globalNamespaceName = GetProject().Properties.Item("DefaultNamespace").Value as string;
            guidClassName = VsctComments.DefaultGuidListClassName;
            cmdIdListClassName = VsctComments.DefaultPkgCmdIDListClassName;
            supporterPostfix = "String";
            isPublic = false;

            if (args != null && args.Length == 0)
            {
                if (!string.IsNullOrEmpty(args[0]))
                    globalNamespaceName = args[0];

                if (!(args.Length < 2 || string.IsNullOrEmpty(args[1])))
                    guidClassName = args[1];

                if (!(args.Length < 3 || string.IsNullOrEmpty(args[2])))
                    cmdIdListClassName = args[2];

                if (!(args.Length < 4 || string.IsNullOrEmpty(args[3])))
                    supporterPostfix = args[3];

                if (!((args.Length < 5 || string.IsNullOrEmpty(args[4])
                       || string.Compare(args[4], "public", true) != 0)))
                    isPublic = true;
            }
        }

        #region Code Definition

        /// <summary>
        /// Creates new static/partial class definition.
        /// </summary>
        protected static CodeTypeDeclaration CreateClass(string name, string comment, bool isPublic)
        {
            CodeTypeDeclaration item = new CodeTypeDeclaration(name);

            item.Comments.Add(new CodeCommentStatement("<summary>", true));
            item.Comments.Add(new CodeCommentStatement(comment, true));
            item.Comments.Add(new CodeCommentStatement("</summary>", true));
            item.IsClass = true;
            item.IsPartial = true;
            if (isPublic)
                item.TypeAttributes = TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.Abstract;
            item.TypeAttributes |= TypeAttributes.BeforeFieldInit | TypeAttributes.Class;

            return item;
        }

        /// <summary>
        /// Creates new constant field with given name and value.
        /// </summary>
        private static CodeMemberField CreateConstField(string type, string name, string value, bool fieldRef)
        {
            CodeMemberField item = new CodeMemberField(new CodeTypeReference(type), name);

            item.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            if (fieldRef)
            {
                item.InitExpression = new CodePrimitiveExpression(value);
            }
            else
            {
                item.InitExpression = new CodeSnippetExpression(value);
            }

            return item;
        }

        /// <summary>
        /// Creates new static/read-only field with given name and value.
        /// </summary>
        private static CodeMemberField CreateStaticField(string type, string name, object value, bool fieldRef)
        {
            CodeMemberField item = new CodeMemberField(new CodeTypeReference(type), name);
            CodeExpression param = fieldRef
                                       ? (CodeExpression) new CodeSnippetExpression((string) value)
                                       : new CodePrimitiveExpression(value);
            item.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            item.InitExpression = new CodeObjectCreateExpression(new CodeTypeReference(type), param);

            return item;
        }

        #endregion
    }
}
