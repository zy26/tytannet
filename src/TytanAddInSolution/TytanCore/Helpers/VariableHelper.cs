using System.CodeDom;
using System.Collections.Generic;
using EnvDTE;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Core.Data.Refactoring;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Class that gives additional help for naming conventions.
    /// </summary>
    public static class VariableHelper
    {
        /// <summary>
        /// Generates the property description based on the variable.
        /// It will automatically update the variable inside the editor.
        /// </summary>
        public static CodeMemberProperty GetProperty(string className, string propertyName, CodeVariable var, string newVariableName, PropertyGeneratorOptions options)
        {
            CodeMemberProperty property = new CodeMemberProperty();

            property.Type = new CodeTypeReference();
            property.Type.BaseType = " " + var.Type.AsString;
            property.Name = propertyName;
            property.Attributes = GetPropertyAttributes(var, options);

            if (var.IsShared)
            {
                // generate getter:
                if ((options & PropertyGeneratorOptions.Getter) == PropertyGeneratorOptions.Getter)
                    property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(className), newVariableName)));

                // generate setter:
                if ((options & PropertyGeneratorOptions.Setter) == PropertyGeneratorOptions.Setter)
                    property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(className), newVariableName),
                                new CodePropertySetValueReferenceExpression()));
            }
            else
            {
                // generate getter:
                if ((options & PropertyGeneratorOptions.Getter) == PropertyGeneratorOptions.Getter)
                    property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), newVariableName)));

                // generate setter:
                if ((options & PropertyGeneratorOptions.Setter) == PropertyGeneratorOptions.Setter)
                    property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), newVariableName),
                                new CodePropertySetValueReferenceExpression()));
            }

            // set comments:
            if ((options & PropertyGeneratorOptions.SuppressComment) != PropertyGeneratorOptions.SuppressComment)
            {
                string commentFormat;
                switch (options & PropertyGeneratorOptions.GetterAndSetter)
                {
                    default:
                        commentFormat = "Gets or sets the value of {0}.";
                        break;
                    case PropertyGeneratorOptions.Getter:
                        commentFormat = "Gets the value of {0}.";
                        break;
                    case PropertyGeneratorOptions.Setter:
                        commentFormat = "Sets the value of {0}.";
                        break;
                }

                property.Comments.Add(new CodeCommentStatement("<summary>", true));
                property.Comments.Add(new CodeCommentStatement(string.Format(commentFormat, property.Name), true));
                property.Comments.Add(new CodeCommentStatement("</summary>", true));
            }

            return property;
        }

        /// <summary>
        /// Gets the access attribute value for given variable depending on the options.
        /// Returns 'true' if new access value is different than the currently stored one.
        /// Otherwise no action may be taken as variable doesn't need to be changed.
        /// </summary>
        public static bool GetVariableAttributes(CodeVariable var, PropertyGeneratorOptions options, out vsCMAccess access)
        {
            switch (options & PropertyGeneratorOptions.ForceVarMask)
            {
                case PropertyGeneratorOptions.ForceVarDontChange: access = vsCMAccess.vsCMAccessDefault; return false;
                case PropertyGeneratorOptions.ForceVarPublic: access = vsCMAccess.vsCMAccessPublic; break;
                case PropertyGeneratorOptions.ForceVarInternal: access = vsCMAccess.vsCMAccessAssemblyOrFamily; break;
                case PropertyGeneratorOptions.ForceVarProtected: access = vsCMAccess.vsCMAccessProtected; break;
                case PropertyGeneratorOptions.ForceVarProtectedInternal: access = vsCMAccess.vsCMAccessProjectOrProtected; break;
                case PropertyGeneratorOptions.ForceVarPrivate: access = vsCMAccess.vsCMAccessPrivate; break;

                default:
                    // by default decrease to private:
                    access = vsCMAccess.vsCMAccessPrivate;
                    break;
            }


            // validate if real change has been made:
            return var.Access != access;
        }

        /// <summary>
        /// Gets the access attributes of the proprety based on the variable and the given options.
        /// </summary>
        public static MemberAttributes GetPropertyAttributes(CodeVariable var, PropertyGeneratorOptions options)
        {
            MemberAttributes attributes = MemberAttributes.Final;

            if (var.IsShared)
                attributes |= MemberAttributes.Static;

            switch (options & PropertyGeneratorOptions.ForcePropertyMask)
            {
                case PropertyGeneratorOptions.ForcePropertyAsVar:
                    {
                        switch (var.Access)
                        {
                            case vsCMAccess.vsCMAccessPrivate: attributes |= MemberAttributes.Private; break;
                            case vsCMAccess.vsCMAccessProtected: attributes |= MemberAttributes.Family; break;
                            case vsCMAccess.vsCMAccessProject:
                            case vsCMAccess.vsCMAccessProjectOrProtected:
                            case vsCMAccess.vsCMAccessAssemblyOrFamily:
                                attributes |= MemberAttributes.FamilyOrAssembly;
                                break;
                            default:
                                attributes |= MemberAttributes.Public;
                                break;
                        }
                    }
                    break;

                case PropertyGeneratorOptions.ForcePropertyPublic: attributes |= MemberAttributes.Public; break;
                case PropertyGeneratorOptions.ForcePropertyInternal: attributes |= MemberAttributes.Assembly; break;
                case PropertyGeneratorOptions.ForcePropertyProtected: attributes |= MemberAttributes.Family; break;
                case PropertyGeneratorOptions.ForcePropertyProtectedInternal: attributes |= MemberAttributes.FamilyOrAssembly; break;
                case PropertyGeneratorOptions.ForcePropertyPrivate: attributes |= MemberAttributes.Private; break;

                default:
                    {
                        // by default generated increased one level:
                        switch (var.Access)
                        {
                            case vsCMAccess.vsCMAccessPrivate:
                            case vsCMAccess.vsCMAccessProtected:
                                attributes |= MemberAttributes.Assembly;
                                break;
                            case vsCMAccess.vsCMAccessProject:
                            case vsCMAccess.vsCMAccessProjectOrProtected:
                            case vsCMAccess.vsCMAccessAssemblyOrFamily:
                                attributes |= MemberAttributes.FamilyOrAssembly;
                                break;
                            default:
                                attributes |= MemberAttributes.Public;
                                break;
                        }
                    }
                    break;
            }

            return attributes;
        }

        /// <summary>
        /// Create constructor <c>CodeElement</c>.
        /// </summary>
        private static CodeMemberMethod CreateConstructor(string codeClassName, CodeModelLanguages language, bool hasVariables)
        {
            CodeMemberMethod initConstructor;

            if (language == CodeModelLanguages.VisualBasic)
            {
                // for VisualBasic work with CodeConstructor class:
                initConstructor = new CodeConstructor();
            }
            else
            {
                // for C# and others work with CodeMemberMethod as in another way this constructor
                // will not have a name:
                initConstructor = new CodeMemberMethod();
            }

            initConstructor.Name = codeClassName;
            initConstructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;

            initConstructor.ReturnType = new CodeTypeReference(" ");

            if (hasVariables)
            {
                // add comment:
                initConstructor.Comments.Add(new CodeCommentStatement("<summary>", true));
                initConstructor.Comments.Add(new CodeCommentStatement("Init constructor of " + codeClassName + ".", true));
                initConstructor.Comments.Add(new CodeCommentStatement("</summary>", true));
            }
            else
            {
                // add comment:
                initConstructor.Comments.Add(new CodeCommentStatement("<summary>", true));
                initConstructor.Comments.Add(new CodeCommentStatement("Default constructor of " + codeClassName + ".", true));
                initConstructor.Comments.Add(new CodeCommentStatement("</summary>", true));
            }

            return initConstructor;
        }

        /// <summary>
        /// Generates the init constructor that is based on the given variables.
        /// </summary>
        public static CodeTypeMember GetInitConstructor(string codeClassName, IList<CodeVariable> vars, IList<string> paramNames, CodeModelLanguages language)
        {
            CodeMemberMethod initConstructor = CreateConstructor(codeClassName, language, vars != null && vars.Count > 0);

            int i = 0;

            if (vars != null)
            {
                //// if all the variables are shared, then make assumption
                //// that the whole class is shared:
                //if (VariableHelper.AreShared(vars))
                //    initConstructor.Attributes = MemberAttributes.Static | MemberAttributes.Final;

                // generate names for parameters if not given:
                if (paramNames == null)
                    paramNames = NameHelper.GetParameterNames(vars, language);

                foreach (CodeVariable v in vars)
                {
                    string paramName = paramNames[i++];

                    // declare parameter:
                    initConstructor.Parameters.Add(new CodeParameterDeclarationExpression(" " + v.Type.AsString, paramName));

                    // make usage of this parameter:
                    if (v.IsShared)
                    {
                        initConstructor.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(codeClassName), v.Name),
                                new CodeVariableReferenceExpression(paramName)));
                    }
                    else
                    {
                        initConstructor.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), v.Name),
                                new CodeVariableReferenceExpression(paramName)));
                    }
                }
            }

            return initConstructor;
        }

        /// <summary>
        /// Generates the init constructor that is based on the given variables.
        /// </summary>
        public static CodeTypeMember GetInitConstructor(string codeClassName, IList<CodeNamedElement> codeElements, CodeModelLanguages language)
        {
            CodeMemberMethod initConstructor = CreateConstructor(codeClassName, language,
                codeElements != null && codeElements.Count > 0);

            if (codeElements != null)
            {
                //// if all the variables are shared, then make assumption
                //// that the whole class is shared:
                //if (VariableHelper.AreShared(vars))
                //    initConstructor.Attributes = MemberAttributes.Static | MemberAttributes.Final;

                foreach (CodeNamedElement e in codeElements)
                {
                    // declare parameter:
                    initConstructor.Parameters.Add(new CodeParameterDeclarationExpression(" " + e.ReferecedTypeName, e.ParameterName));

                    // make usage of this parameter - create assignment instruction:
                    initConstructor.Statements.Add(new CodeAssignStatement(e.GetReferenceExpression(codeClassName),
                            new CodeVariableReferenceExpression(e.ParameterName)));
                }
            }

            return initConstructor;
        }

        /// <summary>
        /// Checks if the collection contains only the shared variables.
        /// </summary>
        public static bool AreShared(IList<CodeVariable> vars)
        {
            if (vars == null)
                return false;

            foreach (CodeVariable v in vars)
                if (!v.IsShared)
                    return false;

            return true;
        }

        /// <summary>
        /// Checks if the collection contains only the shared variables.
        /// </summary>
        public static bool AreShared(IList<CodeNamedElement> codeElements)
        {
            if (codeElements == null)
                return false;

            foreach (CodeNamedElement e in codeElements)
                if (!e.IsStatic)
                    return false;

            return true;
        }

        /// <summary>
        /// Generates the new instance of System.<c>Guid</c> attribute.
        /// </summary>
        public static CodeAttributeDeclaration GetGuidAttribute(string guid)
        {
            return new CodeAttributeDeclaration("System.Runtime.InteropServices.Guid",
                                                new CodeAttributeArgument(new CodePrimitiveExpression(guid.ToUpper())));
        }
    }
}
