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
            string[] args = (string.IsNullOrEmpty(FileNamespace) ? null : FileNamespace.Split(';'));
            string className_GuidList = (args == null || args.Length < 2 || string.IsNullOrEmpty(args[1])
                                             ? VsctComments.DefaultGuidListClassName
                                             : args[1]);
            string className_PkgCmdIDList = (args == null || args.Length < 3 || string.IsNullOrEmpty(args[2])
                                             ? VsctComments.DefaultPkgCmdIDListClassName
                                             : args[2]);
            CodeTypeDeclaration classGuideList = CreateClass(className_GuidList, VsctComments.ClassGuideListComment);
            CodeTypeDeclaration classPkgCmdIDList = CreateClass(className_PkgCmdIDList, VsctComments.ClassPkgCmdIDListComments);
            
            CodeNamespace globalNamespace = (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0])
                                                 ? new CodeNamespace(GetProject().Properties.Item("DefaultNamespace").Value as string)
                                                 : new CodeNamespace(args[0]));
            CodeModelLanguages currentLanguage = CodeHelper.GetCodeLanguage(GetProject().CodeModel.Language);
            IList<NamedValue> guids;
            IList<NamedValue> ids;

            // retrieve the list GUIDs and IDs defined inside VSCT file:
            VsctParser.Parse(inputFileContent, out guids, out ids);

            // generate members describing GUIDs:
            if (guids != null)
            {
                foreach (NamedValue g in guids)
                    classGuideList.Members.Add(CreateStaticField("Guid", g.Name, g.Value));
            }

            // generate members describing IDs:
            if (ids != null)
            {
                foreach (NamedValue i in ids)
                    classPkgCmdIDList.Members.Add(CreateConstField("System.UInt32", i.Name,
                                                                   ConversionHelper.ToHex(i.Value, currentLanguage)));
            }

            // add all members to final namespace:
            globalNamespace.Imports.Add(new CodeNamespaceImport("System"));
            globalNamespace.Types.Add(classGuideList);
            globalNamespace.Types.Add(classPkgCmdIDList);

            // generate source code:
            return CodeHelper.GenerateFromNamespace(GetCodeProvider(), globalNamespace, true);
        }

        #region Code Definition

        /// <summary>
        /// Creates new static/partial class definition.
        /// </summary>
        protected static CodeTypeDeclaration CreateClass (string name, string comment)
        {
            CodeTypeDeclaration item = new CodeTypeDeclaration(name);

            item.Comments.Add(new CodeCommentStatement("<summary>", true));
            item.Comments.Add(new CodeCommentStatement(comment, true));
            item.Comments.Add(new CodeCommentStatement("</summary>", true));
            item.IsClass = true;
            item.IsPartial = true;
            item.TypeAttributes = TypeAttributes.BeforeFieldInit | TypeAttributes.Class;

            return item;
        }

        /// <summary>
        /// Creates new constant field with given name and value.
        /// </summary>
        private static CodeMemberField CreateConstField(string type, string name, string value)
        {
            CodeMemberField item = new CodeMemberField(new CodeTypeReference(type), name);

            item.Attributes = MemberAttributes.Const | MemberAttributes.Public;
            item.InitExpression = new CodeSnippetExpression(value); // CodePrimitiveExpression(value);

            return item;
        }

        /// <summary>
        /// Creates new static/read-only field with given name and value.
        /// </summary>
        private static CodeMemberField CreateStaticField(string type, string name, object value)
        {
            CodeMemberField item = new CodeMemberField(new CodeTypeReference(type), name);
            item.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            item.InitExpression = new CodeObjectCreateExpression(new CodeTypeReference(type),
                                                              new CodePrimitiveExpression(value));

            return item;
        }

        #endregion
    }
}
