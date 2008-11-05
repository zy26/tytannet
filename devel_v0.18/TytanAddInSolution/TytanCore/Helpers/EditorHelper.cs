using System.Collections.Generic;
using EnvDTE;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Class that gives additional help for operations over IDE editors.
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// Gets the specified type list selected from given elements or null if there is no elements of that type.
        /// </summary>
        public static IList<T> GetList<T>(CodeElements members, vsCMElement type) where T : class
        {
            if (members == null)
                return null;


            try
            {
                IList<T> result = new List<T>();
                foreach (CodeElement e in members)
                {
                    if (e.Kind == type)
                        result.Add(e as T);
                }

                if (result.Count == 0)
                    return null;

                return result;
            }
            catch
            {
                return null;
            }
        }

        private static bool Fits(int value, int min, int max)
        {
            return value <= max && value >= min;
        }

        /// <summary>
        /// Gets the specified type list of selected from given elements that are between particular code lines or null
        /// if there is no elements of that type.
        /// </summary>
        public static IList<T> GetList<T>(CodeElements members, vsCMElement type, int minLine, int maxLine) where T : class
        {
            if ( members == null)
                return null;

            try
            {
                IList<T> result = new List<T>();
                foreach (CodeElement e in members)
                {
                    if (e.Kind == type &&
                        (Fits(e.StartPoint.Line, minLine, maxLine) || Fits(e.EndPoint.Line, minLine, maxLine)))
                        result.Add(e as T);
                }

                if (result.Count == 0)
                    return null;

                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the specified type list of selected elements or null if there is no elements of that type selected.
        /// </summary>
        public static IList<T> GetList<T>(CodeElements members, vsCMElement type, TextSelection selection) where T : class
        {
            int minLine = selection.TopLine;
            int maxLine = (selection.BottomPoint.LineCharOffset > 1 ? selection.BottomPoint.Line : selection.BottomPoint.Line - 1);

            return GetList<T>(members, type, minLine, maxLine);
        }

        /// <summary>
        /// Checks if there is an element with given name on the list.
        /// </summary>
        public static bool Contains(IList<CodeProperty> list, string name)
        {
            foreach (CodeProperty i in list)
                if (i.Name == name)
                    return true;

            return false;
        }

        /// <summary>
        /// Returns the supported state of the command if it is not a designer window.
        /// Otherwise or if no document is selected returns not-supported.
        /// </summary>
        public static vsCommandStatus GetDefaultStatusForNonDesigner(Window activeWindow, Document activeDocument)
        {
            // check if current document is contained in a project:
            if (activeDocument != null &&
                activeDocument.ProjectItem != null &&
                activeWindow != null && activeWindow.Document != null
                && activeWindow.Document.ProjectItem != null)
            {
                // check if current window is not a designer:
                if (!(activeWindow.Object is System.ComponentModel.Design.IDesignerHost))
                    return
                        (vsCommandStatus)
                        ((int) vsCommandStatus.vsCommandStatusSupported | (int) vsCommandStatus.vsCommandStatusEnabled);
            }

            return
                (vsCommandStatus)
                ((int) vsCommandStatus.vsCommandStatusUnsupported | (int) vsCommandStatus.vsCommandStatusInvisible);
        }

        /// <summary>
        /// Returns the supported state of the command if it is a part of the project.
        /// Otherwise or if no document is selected returns not-supported.
        /// </summary>
        public static vsCommandStatus GetDefaultStatus(Solution activeSolution, Window activeWindow, Document activeDocument)
        {
            // check if current document is contained in a project:
            if (activeSolution != null &&
                activeSolution.Projects != null &&
                activeSolution.Projects.Count > 0)
            {
                return
                    (vsCommandStatus)
                    ((int) vsCommandStatus.vsCommandStatusSupported | (int) vsCommandStatus.vsCommandStatusEnabled);
            }

            return
                (vsCommandStatus)
                ((int) vsCommandStatus.vsCommandStatusUnsupported | (int) vsCommandStatus.vsCommandStatusInvisible);
        }

        /// <summary>
        /// Gets the currently selected variables inside the code editor.
        /// </summary>
        public static Data.CodeEditSelection GetSelectedVariables(Data.CodeEditPoint editorEditPoint)
        {
            CodeVariable codeVariable;
            CodeClass codeClass;
            CodeStruct codeStruct;
            bool partialSelection = true;

            IList<CodeVariable> vars = null;

            // get the variable and class at cursor in the IDE:
            codeVariable = editorEditPoint.GetCurrentCodeElement<CodeVariable>(vsCMElement.vsCMElementVariable);
            codeClass = editorEditPoint.GetCurrentCodeElement<CodeClass>(vsCMElement.vsCMElementClass);
            codeStruct = editorEditPoint.GetCurrentCodeElement<CodeStruct>(vsCMElement.vsCMElementStruct);

            if (codeClass != null && codeStruct != null)
            {
                if (codeClass.GetStartPoint(vsCMPart.vsCMPartBody).Line < codeStruct.GetStartPoint(vsCMPart.vsCMPartBody).Line)
                    codeClass = null;
                else
                    if (codeStruct.GetStartPoint(vsCMPart.vsCMPartBody).Line < codeClass.GetStartPoint(vsCMPart.vsCMPartBody).Line)
                        codeStruct = null;
            }

            // if text selected:
            if (editorEditPoint.Selection.TopLine != editorEditPoint.Selection.BottomLine)
            {
                // get all the variables that are selected:
                if (codeClass != null)
                {
                    vars = GetList<CodeVariable>(codeClass.Members, vsCMElement.vsCMElementVariable, editorEditPoint.Selection);
                    if (vars != null)
                        codeClass = vars[0].Parent as CodeClass;
                }
                else
                    if (codeStruct != null)
                    {
                        vars = GetList<CodeVariable>(codeStruct.Members, vsCMElement.vsCMElementVariable, editorEditPoint.Selection);
                        if (vars != null)
                            codeStruct = vars[0].Parent as CodeStruct;
                    }
            }
            else
            {
                // if there is only one variable selected:
                if (codeVariable != null && codeVariable.StartPoint.Line == editorEditPoint.EditPoint.Line)
                {
                    vars = new List<CodeVariable>();
                    vars.Add(codeVariable);

                    codeClass = codeVariable.Parent as CodeClass;
                    codeStruct = codeVariable.Parent as CodeStruct;
                }
                else
                    // otherwise the the whole class will be changed to properties:
                    if (codeClass != null)
                    {
                        vars = GetList<CodeVariable>(codeClass.Members, vsCMElement.vsCMElementVariable);
                        partialSelection = false;
                    }
                    else
                        // or struct
                        if (codeStruct != null)
                        {
                            vars = GetList<CodeVariable>(codeStruct.Members, vsCMElement.vsCMElementVariable);
                            partialSelection = false;
                        }
            }

            // validate is something has been found:
            if (codeClass != null || codeStruct != null)
            {
                return new Data.CodeEditSelection(codeClass, codeStruct, vars, null, partialSelection);
            }

            return null;
        }

        /// <summary>
        /// Gets the currently selected methods inside the code editor.
        /// </summary>
        public static Data.CodeEditSelection GetSelectedMethods(Data.CodeEditPoint editorEditPoint, bool oneMethodAllowed)
        {
            CodeFunction codeMethod;
            CodeClass codeClass;
            CodeStruct codeStruct;
            bool partialSelection = true;

            IList<CodeFunction> methods = null;

            // get the method and class at cursor in the IDE:
            codeMethod = editorEditPoint.GetCurrentCodeElement<CodeFunction>(vsCMElement.vsCMElementFunction);
            codeClass = editorEditPoint.GetCurrentCodeElement<CodeClass>(vsCMElement.vsCMElementClass);
            codeStruct = editorEditPoint.GetCurrentCodeElement<CodeStruct>(vsCMElement.vsCMElementStruct);

            if (codeClass != null && codeStruct != null)
            {
                if (codeClass.GetStartPoint(vsCMPart.vsCMPartBody).Line < codeStruct.GetStartPoint(vsCMPart.vsCMPartBody).Line)
                    codeClass = null;
                else
                    if (codeStruct.GetStartPoint(vsCMPart.vsCMPartBody).Line < codeClass.GetStartPoint(vsCMPart.vsCMPartBody).Line)
                        codeStruct = null;
            }

            // if there is only one method is selected:
            if (oneMethodAllowed && codeMethod != null && (codeMethod.StartPoint.Line <= editorEditPoint.Selection.TopPoint.Line && codeMethod.EndPoint.Line >= editorEditPoint.Selection.BottomPoint.Line))
            {
                methods = new List<CodeFunction>();
                methods.Add(codeMethod);

                codeClass = codeMethod.Parent as CodeClass;
                codeStruct = codeMethod.Parent as CodeStruct;
            }
            else
            {
                // if text selected:
                if (editorEditPoint.Selection.TopLine != editorEditPoint.Selection.BottomLine)
                {
                    // get all the methods that are selected:
                    if (codeClass != null)
                    {
                        methods = GetList<CodeFunction>(codeClass.Members, vsCMElement.vsCMElementFunction, editorEditPoint.Selection.TopPoint.Line, editorEditPoint.Selection.BottomPoint.Line);
                        if (methods != null)
                            codeClass = methods[0].Parent as CodeClass;
                    }
                    else
                        if (codeStruct != null)
                        {
                            methods = GetList<CodeFunction>(codeStruct.Members, vsCMElement.vsCMElementFunction, editorEditPoint.Selection);
                            if (methods != null)
                                codeStruct = methods[0].Parent as CodeStruct;
                        }
                }
                else
                {
                    // otherwise the the whole class will be changed to properties:
                    if (codeClass != null)
                    {
                        methods = GetList<CodeFunction>(codeClass.Members, vsCMElement.vsCMElementFunction);
                        partialSelection = false;
                    }
                    else
                        // or struct
                        if (codeStruct != null)
                        {
                            methods = GetList<CodeFunction>(codeStruct.Members, vsCMElement.vsCMElementFunction);
                            partialSelection = false;
                        }
                }
            }

            // validate is something has been found:
            if (codeClass != null || codeStruct != null)
            {
                return new Data.CodeEditSelection(codeClass, codeStruct, null, methods, partialSelection);
            }
            
            return null;
        }

        /// <summary>
        /// Filters specified collection of methods and removes the given types.
        /// </summary>
        public static IList<CodeFunction> FilterMethods(IList<CodeFunction> methods, params vsCMFunction[] removeTypes)
        {
            IList<CodeFunction> result = null;

            if (methods != null && methods.Count > 0)
            {
                if (removeTypes != null && removeTypes.Length > 0)
                {
                    foreach (CodeFunction m in methods)
                    {
                        if (!IsMethodType(m.FunctionKind, removeTypes))
                        {
                            if (result == null)
                                result = new List<CodeFunction>();

                            result.Add(m);
                        }
                    }
                }
                else
                    // do nothing if no valid params specified:
                    result = methods;
            }

            return result;
        }

        /// <summary>
        /// Checks if given type belongs to particular list.
        /// </summary>
        private static bool IsMethodType(vsCMFunction type, vsCMFunction[] types)
        {
            for (int i = 0; i < types.Length; i++)
                if (type == types[i])
                    return true;

            return false;
        }
    }
}
