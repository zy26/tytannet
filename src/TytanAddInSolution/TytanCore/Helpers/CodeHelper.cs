using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Pretorianie.Tytan.Core.Interfaces;
using CodeNamespace=System.CodeDom.CodeNamespace;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Class that helps in source-code generation.
    /// </summary>
    public static class CodeHelper
    {
        private static readonly char[] NewLineChars = new char[] {'\r', '\n'};

        /// <summary>
        /// Generates source code from given CodeStatement.
        /// </summary>
        public static string GenerateFromStatement(CodeDomProvider codeProvider, CodeStatement statement)
        {
            StringBuilder result = new StringBuilder();
            StringWriter writer = new StringWriter(result);

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = false;
            options.BracingStyle = "C";

            // generate the code:
            codeProvider.GenerateCodeFromStatement(statement, writer, options);

            // send it to the StringBuilder object:
            writer.Flush();


            return result.ToString();
        }

        /// <summary>
        /// Generates source code from given CodeCompileUnit.
        /// </summary>
        public static string GenerateFromCompileUnit(CodeDomProvider codeProvider, CodeCompileUnit unit)
        {
            StringBuilder result = new StringBuilder();
            StringWriter writer = new StringWriter(result);

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = false;
            options.BracingStyle = "C";
            options.VerbatimOrder = true;

            // generate the code:
            codeProvider.GenerateCodeFromCompileUnit(unit, writer, options);

            // send it to the StringBuilder object:
            writer.Flush();


            return result.ToString();
        }

        /// <summary>
        /// Generates source code from given CodeTypeMemberCollection.
        /// </summary>
        public static string GenerateFromMember(CodeDomProvider codeProvider, CodeTypeMemberCollection memberCollection, bool blankLinesBetweenMembers)
        {
            StringBuilder result = new StringBuilder();
            StringWriter writer = new StringWriter(result);

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = blankLinesBetweenMembers;
            options.ElseOnClosing = true;
            options.VerbatimOrder = true;
            options.BracingStyle = "C";

            foreach (CodeTypeMember typeMember in memberCollection)
            {
                // generate the code:
                codeProvider.GenerateCodeFromMember(typeMember, writer, options);

                // send it to the StringBuilder object:
                writer.Flush();
            }

            return result.ToString();
        }

        /// <summary>
        /// Generates source code from given namespace.
        /// </summary>
        public static string GenerateFromNamespace(CodeDomProvider codeProvider, CodeNamespace codeNamespace, bool blankLinesBetweenMembers)
        {
            StringBuilder result = new StringBuilder();
            StringWriter writer = new StringWriter(result);

            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BlankLinesBetweenMembers = blankLinesBetweenMembers;
            options.ElseOnClosing = true;
            options.VerbatimOrder = true;
            options.BracingStyle = "C";

            // generate the code:
            codeProvider.GenerateCodeFromNamespace(codeNamespace, writer, options);

            // send it to the StringBuilder object:
            writer.Flush();

            return result.ToString();
        }

        /// <summary>
        /// Generates source code from given CodeTypeMemberCollection.
        /// </summary>
        public static string GenerateFromMember(CodeModelLanguages language, CodeTypeMemberCollection memberCollection)
        {
            return GenerateFromMember(GetCodeProvider(language), memberCollection, true);
        }

        /// <summary>
        /// Generates source code from given CodeTypeMemberCollection.
        /// </summary>
        public static string GenerateFromMember(CodeModelLanguages language, CodeTypeMemberCollection memberCollection, bool blankLinesBetweenMembers)
        {
            return GenerateFromMember(GetCodeProvider(language), memberCollection, blankLinesBetweenMembers);
        }

        /// <summary>
        /// Generates source code from given attributes.
        /// </summary>
        public static string GenerateFromAttribute(CodeModelLanguages language, params CodeAttributeDeclaration[] attributes)
        {
            return GenerateFromAttribute(GetCodeProvider(language), attributes);
        }

        /// <summary>
        /// Generates source code from given attributes.
        /// </summary>
        public static string GenerateFromAttribute(CodeDomProvider codeProvider, params CodeAttributeDeclaration[] attributes)
        {
            CodeTypeMemberCollection collection = new CodeTypeMemberCollection();
            CodeMemberMethod temp = new CodeMemberMethod();
            int start = 0;

            if (attributes == null || attributes.Length == 0)
                return string.Empty;

            temp.Name = " ";
            temp.ReturnType = new CodeTypeReference(" ");
            collection.Add(temp);

            foreach (CodeAttributeDeclaration a in attributes)
                temp.CustomAttributes.Add(a);

            // get the code from function with attributes:
            string code = GenerateFromMember(codeProvider, collection, false);

            // and remove the last line with the function declaration:
            for (int i = 0; i < attributes.Length; i++)
            {
                start = code.IndexOfAny(NewLineChars, start);
            }
            if (start != -1)
                code = code.Substring(0, start);

            return code;
        }

        #region Code Providers

        /// <summary>
        /// Returns a CodeDomProvider object for the given language.
        /// </summary>
        public static CodeDomProvider GetCodeProvider(CodeModelLanguages language)
        {
            switch (language)
            {
                case CodeModelLanguages.VisualBasic:
                    return CodeDomProvider.CreateProvider("VB");
                case CodeModelLanguages.VisualJSharp:
                    return CodeDomProvider.CreateProvider("VJ#");
                case CodeModelLanguages.VisualC:
                case CodeModelLanguages.VisualManagedC:
                    return CodeDomProvider.CreateProvider("MC");
                default:
                    return CodeDomProvider.CreateProvider("C#");
            }
        }

        /// <summary>
        /// Returns a CodeDomProvider object for the language of the project item Language property.
        /// </summary>
        public static CodeDomProvider GetCodeProvider(string languageGuid)
        {
            switch (languageGuid)
            {
                case CodeModelLanguageConstants.vsCMLanguageVB:
                    return CodeDomProvider.CreateProvider("VB");
                case CodeModelLanguageConstants2.vsCMLanguageJSharp:
                    return CodeDomProvider.CreateProvider("VJ#");
                case CodeModelLanguageConstants.vsCMLanguageVC:
                case CodeModelLanguageConstants.vsCMLanguageMC:
                    return CodeDomProvider.CreateProvider("MC");
                default:
                    return CodeDomProvider.CreateProvider("C#");
            }
        }

        /// <summary>
        /// Returns a string name for language of the project item Language property.
        /// It can be later used to ask for CodeDomProvider.
        /// </summary>
        public static CodeModelLanguages GetCodeLanguage(string languageGuid)
        {
            switch (languageGuid)
            {
                case CodeModelLanguageConstants.vsCMLanguageVB:
                    return CodeModelLanguages.VisualBasic;
                case CodeModelLanguageConstants2.vsCMLanguageJSharp:
                    return CodeModelLanguages.VisualJSharp;
                case CodeModelLanguageConstants.vsCMLanguageVC:
                    return CodeModelLanguages.VisualC;
                case CodeModelLanguageConstants.vsCMLanguageMC:
                    return CodeModelLanguages.VisualManagedC;
                default:
                    return CodeModelLanguages.VisualCSharp;
            }
        }

        #endregion
    }
}
