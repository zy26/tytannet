using System.Collections.Generic;
using EnvDTE;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Parameters retrieved from the editor window that describe currently selected items. 
    /// </summary>
    public class CodeEditSelection
    {
        private readonly CodeClass parentClass;
        private readonly CodeStruct parentStruct;
        private readonly IList<CodeVariable> variables;
        private readonly IList<CodeFunction> methods;
        private readonly IList<CodeProperty> properties;
        private readonly string parentName;
        private readonly string parentLanguage;
        private readonly CodeElements codeMembers;

        private readonly bool partialSelection;
        private IList<CodeVariable> allVars;
        private IList<CodeVariable> disabledVars;
        private IList<CodeFunction> allMethods;
        private IList<CodeFunction> disabledMethods;
        private IList<CodeProperty> allProperties;
        private IList<CodeProperty> disabledProperties;

        /// <summary>
        /// Init constructor of CodeEditSelection.
        /// </summary>
        public CodeEditSelection(CodeClass parentClass, CodeStruct parentStruct, IList<CodeVariable> variables, IList<CodeFunction> methods, IList<CodeProperty> properties, bool partialSelection)
        {
            this.parentClass = parentClass;
            this.parentStruct = parentStruct;
            this.variables = variables;
            this.methods = methods;
            this.properties = properties;
            this.partialSelection = partialSelection;

            if (parentClass != null)
            {
                parentName = parentClass.Name;
                parentLanguage = parentClass.Language;
                codeMembers = parentClass.Members;
            }
            else
                if (parentStruct != null)
                {
                    parentName = parentStruct.Name;
                    parentLanguage = parentStruct.Language;
                    codeMembers = parentStruct.Members;
                }
        }

        private void CalculateDisabledVariables()
        {
            if (allVars != null)
                return;

            allVars = EditorHelper.GetList<CodeVariable>(codeMembers, vsCMElement.vsCMElementVariable);

            // evaluate not selected variables:
            if (allVars != null)
            {
                foreach (CodeVariable v in allVars)
                {
                    if (variables == null || !variables.Contains(v))
                    {
                        if (disabledVars == null)
                            disabledVars = new List<CodeVariable>();

                        disabledVars.Add(v);
                    }
                }
            }
        }

        private void CalculateDisabledMethods()
        {
            if (allMethods != null)
                return;

            allMethods = EditorHelper.GetList<CodeFunction>(codeMembers, vsCMElement.vsCMElementFunction);

            // evaluate not selected methods:
            if (allMethods != null)
                foreach (CodeFunction m in allMethods)
                {
                    if (methods == null || !methods.Contains(m))
                    {
                        if (disabledMethods == null)
                            disabledMethods = new List<CodeFunction>();

                        disabledMethods.Add(m);
                    }
                }
        }

        private void CalculateDisabledProperties()
        {
            if (allProperties != null)
                return;

            allProperties = EditorHelper.GetList<CodeProperty>(codeMembers, vsCMElement.vsCMElementProperty);

            // evaluate not selected properties:
            if (allProperties != null)
                foreach (CodeProperty p in allProperties)
                {
                    if (properties == null || !properties.Contains(p))
                    {
                        if (disabledProperties == null)
                            disabledProperties = new List<CodeProperty>();

                        disabledProperties.Add(p);
                    }
                }
        }

        #region Properties

        /// <summary>
        /// Gets the parent class.
        /// </summary>
        public CodeClass ParentClass
        {
            get
            {
                return parentClass;
            }
        }

        /// <summary>
        /// Gets the parent structure.
        /// </summary>
        public CodeStruct ParentStruct
        {
            get
            {
                return parentStruct;
            }
        }

        /// <summary>
        /// Gets the list of variables.
        /// </summary>
        public IList<CodeVariable> Variables
        {
            get
            {
                return variables;
            }
        }

        /// <summary>
        /// Gets the list of methods.
        /// </summary>
        public IList<CodeFunction> Methods
        {
            get
            {
                return methods;
            }
        }

        /// <summary>
        /// Gets the list of properties.
        /// </summary>
        public IList<CodeProperty> Properties
        {
            get
            {
                return properties;
            }
        }

        /// <summary>
        /// Gets the parent name.
        /// </summary>
        public string ParentName
        {
            get
            {
                return parentName;
            }
        }

        /// <summary>
        /// Gets the parent language.
        /// </summary>
        public string ParentLanguage
        {
            get
            {
                return parentLanguage;
            }
        }

        /// <summary>
        /// Gets the stored CodeElements.
        /// </summary>
        public CodeElements CodeMembers
        {
            get
            {
                return codeMembers;
            }
        }

        /// <summary>
        /// Checks if specified list of variables or methods is only a partial
        /// collection of the whole possible parent's elements with given type.
        /// </summary>
        public bool IsPartialSelection
        {
            get
            {
                return partialSelection;
            }
        }

        /// <summary>
        /// Gets the list of all variables.
        /// </summary>
        public IList<CodeVariable> AllVariables
        {
            get
            {
                CalculateDisabledVariables();
                return allVars;
            }
        }

        /// <summary>
        /// Gets the list of variables that not belong to Variables, but do exist inside AllVariables.
        /// </summary>
        public IList<CodeVariable> DisabledVariables
        {
            get
            {
                CalculateDisabledVariables();
                return disabledVars;
            }
        }

        /// <summary>
        /// Gets all the methods.
        /// </summary>
        public IList<CodeFunction> AllMethods
        {
            get
            {
                CalculateDisabledMethods();
                return allMethods;
            }
        }

        /// <summary>
        /// Gets the list of methods that not belong to Methods, but do exist inside AllMethods.
        /// </summary>
        public IList<CodeFunction> DisabledMethods
        {
            get
            {
                CalculateDisabledMethods();
                return disabledMethods;
            }
        }

        /// <summary>
        /// Gets all the properties.
        /// </summary>
        public IList<CodeProperty> AllProperties
        {
            get
            {
                CalculateDisabledProperties();
                return allProperties;
            }
        }

        /// <summary>
        /// Gets the list of properties that not belong to Properties, but do exist inside AllProperties.
        /// </summary>
        public IList<CodeProperty> DisabledProperties
        {
            get
            {
                CalculateDisabledProperties();
                return disabledProperties;
            }
        }

        #endregion

        #region Start/End Point

        /// <summary>
        /// Get EndPoint of specified element of parent object.
        /// </summary>
        public TextPoint GetStartPoint(vsCMPart part)
        {
            if (parentClass != null)
                return parentClass.GetStartPoint(part);
            else
                if (parentStruct != null)
                    return parentStruct.GetStartPoint(part);

            return null;
        }

        /// <summary>
        /// Get EndPoint of specified element of parent object.
        /// </summary>
        public TextPoint GetEndPoint(vsCMPart part)
        {
            if (parentClass != null)
                return parentClass.GetEndPoint(part);
            else
                if (parentStruct != null)
                    return parentStruct.GetEndPoint(part);

            return null;
        }

        #endregion

        #region Parent Access

        /// <summary>
        /// Gets the specified type list from given of parent's CodeElements or null if there is no elements of that type.
        /// </summary>
        public IList<T> GetParentMembers<T>(vsCMElement type) where T : class
        {
            if (CodeMembers != null)
                return EditorHelper.GetList<T>(CodeMembers, type);
            else
                return null;
        }

        #endregion

    }
}
