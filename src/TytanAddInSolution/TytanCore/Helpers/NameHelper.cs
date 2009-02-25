using System.Collections.Generic;
using System.Text;
using EnvDTE;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Core.Data.Refactoring;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class that operates over the names and transforms them to Camel/Pascal notation.
    /// </summary>
    public static class NameHelper
    {
        private static bool RemoveLeadingUnderscore(string name, StringBuilder result, bool toUpper)
        {
            char firstChar;
            bool modified = false;
            int i = 0;

            if (name.StartsWith("_"))
            {
                while (name[i++] == '_')
                    result.Remove(0, 1);
                modified = true;
            }
            else
            {
                if (name.StartsWith("m_") || name.StartsWith("M_") || name.StartsWith("g_") || name.StartsWith("G_"))
                {
                    result.Remove(0, 2);
                    modified = true;
                }
            }

            // convert first letter to upper one:
            if (toUpper)
                firstChar = char.ToUpper(result[0]);
            else firstChar = char.ToLower(result[0]);

            if (firstChar != result[0])
            {
                result[0] = firstChar;
                return true;
            }

            return modified;
        }

        private static void RemoveInternalUnderscore(string name, StringBuilder result)
        {
            int i = 0;
            int found = 0;

            // repeat removing till there is no more underscore:
            while ((i = name.IndexOf('_', i)) >= 0)
            {
                // remove the char from result and make the next one upper-case:
                int position = i - found;
                result.Remove(position, 1);
                result[position] = char.ToUpper(result[position]);
                
                found++;
                i++;
            }
        }

        /// <summary>
        /// Gets the name of the property based on a given variable name.
        /// </summary>
        public static string GetPropertyName(string variableName, CodeModelLanguages language)
        {
            StringBuilder result = new StringBuilder(variableName);

            if (RemoveLeadingUnderscore(variableName, result, true))
                RemoveInternalUnderscore(result.ToString(), result);
            else RemoveInternalUnderscore(variableName, result);

            return result.ToString();
        }

        /// <summary>
        /// Gets the normalized name of the variable.
        /// Remember that using this method may generate conflicts if such a name already exists,
        /// and these conflicts should be solved by the caller.
        /// </summary>
        public static string GetVariableName(string variableName, CodeModelLanguages language)
        {
            return char.ToLower(variableName[0]) + variableName.Substring(1);
        }

        /// <summary>
        /// Gets the list of new variables' names and new propertys' names without the name conflicts.
        /// </summary>
        public static void GetVariableNames(IList<CodeVariable> vars, out IList<string> varNames, out IList<string> propNames, bool canChangeVarName, CodeModelLanguages language)
        {
            if (vars == null || vars.Count == 0)
            {
                varNames = null;
                propNames = null;
            }
            else
            {
                varNames = new List<string>(vars.Count);
                propNames = new List<string>(vars.Count);

                // generate the names:
                foreach (CodeVariable v in vars)
                {
                    string propName = GetPropertyName(v.Name, language);

                    // generate unique property name:
                    while (propNames.Contains(propName))
                        propName = propName + "Ex";
                    propNames.Add(propName);

                    // try to generate unique variable name:
                    if (canChangeVarName)
                    {
                        // new variable name will be based on property:
                        string varName = GetVariableName(GetPropertyName(v.Name, language), language);
                        if (string.Compare(propName, varName, language == CodeModelLanguages.VisualBasic) == 0)
                        {
                            do
                            {
                                varName = "_" + GetVariableName(varName, language);
                            } while (string.Compare(propName, varName, true) == 0);
                        }

                        // and if there are already variables with the same name,
                        // it will be prefixed with underline:
                        while (varNames.Contains(varName))
                            varName = "_" + varName;

                        varNames.Add(varName);
                    }
                    else
                    {
                        // in case no one allowed variable name manipulations,
                        // the name will remain untouched:
                        varNames.Add(v.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the name of the variable based on a given variable name that should like property.
        /// Remember that using this method may generate conflicts if such a name already exists,
        /// and these conflicts should be solved by the caller.
        /// </summary>
        public static string GetParameterName(string variableName, CodeModelLanguages language)
        {
            StringBuilder result = new StringBuilder(variableName);

            if (RemoveLeadingUnderscore(variableName, result, false))
                RemoveInternalUnderscore(result.ToString(), result);
            else RemoveInternalUnderscore(variableName, result);
            
            return result.ToString();
        }

        /// <summary>
        /// Gets the names of the paramters based on a given set of variables.
        /// Each variable generates one name.
        /// </summary>
        public static IList<string> GetParameterNames(IList<CodeVariable> vars, CodeModelLanguages language)
        {
            if (vars == null || vars.Count == 0)
                return null;

            IList<string> paramNames = new List<string>(vars.Count);
            string name;

            // generate name in secure mode:
            foreach (CodeVariable v in vars)
            {
                name = GetParameterName(v.Name, language);

                while (paramNames.Contains(name))
                    name = "_" + name;

                paramNames.Add(name);
            }

            return paramNames;
        }

        /// <summary>
        /// Updates the parameter names for each <c>CodeNamedElement</c> item.
        /// </summary>
        public static void UpdateParameterNames(IList<CodeNamedElement> codeElements, CodeModelLanguages language)
        {
            if (codeElements == null || codeElements.Count == 0)
                return;

            List<string> paramNames = new List<string>();
            string name;

            // generate name in secure mode:
            foreach (CodeNamedElement e in codeElements)
            {
                name = GetParameterName(e.Name, language);

                while (paramNames.Contains(name))
                    name = "_" + name;

                e.ParameterName = name;
                paramNames.Add(name);
            }
        }
    }
}
