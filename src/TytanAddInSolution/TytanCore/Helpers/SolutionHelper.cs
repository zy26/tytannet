using System;
using System.Collections.Generic;
using EnvDTE;
using Pretorianie.Tytan.Core.DbgView;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class that manage the whole Solution.
    /// </summary>
    public static class SolutionHelper
    {
        #region Public Interfaces

        /// <summary>
        /// Inteface implemented by the class that search for classes inside the solution tree.
        /// </summary>
        public interface IClassFinder
        {
            /// <summary>
            /// Checks if current class matches the search criteria.
            /// </summary>
            bool MatchCriteria(CodeClass codeClass);

            /// <summary>
            /// Activate found class element.
            /// </summary>
            void Activate(ProjectItem item, CodeClass codeClass, Window editorWindow, TextSelection editorSelection);
        }

        /// <summary>
        /// Inteface implemented by the class that search for structures inside the solution tree.
        /// </summary>
        public interface IStructFinder
        {
            /// <summary>
            /// Checks if current structure matches the search criteria.
            /// </summary>
            bool MatchCriteria(CodeStruct codeStruct);

            /// <summary>
            /// Activate found structure element.
            /// </summary>
            void Activate(ProjectItem item, CodeStruct codeStruct, Window editorWindow, TextSelection editorSelection);
        }

        /// <summary>
        /// Data describing the location inside the file.
        /// </summary>
        public class ActivationData : EventArgs
        {
            private readonly Window editorWindow;
            private readonly TextSelection editorSelection;
            private readonly int line;
            private readonly int lineCharOffset;

            internal ActivationData(Window editorWindow, TextSelection editorSelection, int line, int lineCharOffset)
            {
                this.editorWindow = editorWindow;
                this.editorSelection = editorSelection;
                this.line = line;
                this.lineCharOffset = lineCharOffset;
            }

            /// <summary>
            /// Jumps to the specified file.
            /// </summary>
            public void Execute()
            {
                editorSelection.MoveToLineAndOffset(line, lineCharOffset, false);
                editorWindow.Visible = true;
            }
        }

        /// <summary>
        /// Search for class/structure with specified name.
        /// </summary>
        public class ClassFinder : IClassFinder, IStructFinder
        {
            private readonly string name;
            private readonly string methodName;
            private readonly bool autoExecute;
            private ActivationData activationData;

            /// <summary>
            /// Init constructor.
            /// Allows to perform a search for given class.
            /// </summary>
            public ClassFinder(string name)
            {
                this.name = name;
                
                methodName = null;
                autoExecute = true;
            }

            /// <summary>
            /// Init constructor.
            /// Allows to perform a search for given method inside specified class.
            /// </summary>
            public ClassFinder(string name, string methodName)
            {
                this.name = name;
                this.methodName = methodName;
                autoExecute = true;
            }

            /// <summary>
            /// Init constructor.
            /// Allows to perform a search for given method inside specified class and jumps inside the code to proper location.
            /// </summary>
            public ClassFinder(string name, string methodName, bool autoExecute)
            {
                this.name = name;
                this.methodName = methodName;
                this.autoExecute = autoExecute;
            }

            #region Properties

            /// <summary>
            /// Gets the name of the class/structure that is searched.
            /// </summary>
            public string Name
            {
                get { return name; }
            }

            /// <summary>
            /// Gets the name of the method to jump.
            /// </summary>
            public string MethodName
            {
                get { return methodName; }
            }

            /// <summary>
            /// Gets the data that describes file location.
            /// </summary>
            public ActivationData ActivationData
            {
                get { return activationData; }
            }

            #endregion

            #region Activation

            private void Activate(Window editorWindow, TextSelection editorSelection, IList<CodeFunction> methods, int line, int lineCharOffset)
            {
                if (methods != null && methods.Count > 0)
                {
                    foreach (CodeFunction m in methods)
                    {
                        if (string.Compare(m.Name, methodName, true) == 0)
                        {
                            line = m.StartPoint.Line;
                            lineCharOffset = m.StartPoint.LineCharOffset;
                            break;
                        }
                    }
                }

                // execute default action or create location description:
                if (autoExecute)
                {
                    editorSelection.MoveToLineAndOffset(line, lineCharOffset, false);
                    editorWindow.Visible = true;
                }
                else
                {
                    activationData = new ActivationData(editorWindow, editorSelection, line, lineCharOffset);
                }
            }

            #endregion

            #region IClassFinder Members

            /// <summary>
            /// Checks if current class matches the search criteria.
            /// </summary>
             public bool MatchCriteria(CodeClass codeClass)
            {
                return string.Compare(codeClass.Name, name, true) == 0;
            }

            /// <summary>
            /// Activate found class element.
            /// </summary>
            public void Activate(ProjectItem item, CodeClass codeClass, Window editorWindow, TextSelection editorSelection)
            {
                Activate(editorWindow, editorSelection, (string.IsNullOrEmpty(methodName) ? null : EditorHelper.GetList<CodeFunction>(codeClass.Members, vsCMElement.vsCMElementFunction)), codeClass.StartPoint.Line, codeClass.StartPoint.LineCharOffset);
            }

            #endregion

            #region IStructFinder Members

            /// <summary>
            /// Checks if current structure matches the search criteria.
            /// </summary>
            public bool MatchCriteria(CodeStruct codeStruct)
            {
                return string.Compare(codeStruct.Name, name, true) == 0;
            }

            /// <summary>
            /// Activate found structure element.
            /// </summary>
            public void Activate(ProjectItem item, CodeStruct codeStruct, Window editorWindow, TextSelection editorSelection)
            {
                Activate(editorWindow, editorSelection, (string.IsNullOrEmpty(methodName) ? null : EditorHelper.GetList<CodeFunction>(codeStruct.Members, vsCMElement.vsCMElementFunction)), codeStruct.StartPoint.Line, codeStruct.StartPoint.LineCharOffset);
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Open code editor window and set the cursor at the start of particular class or structure.
        /// Returns 'true' if editor opened, otherwise 'false'.
        /// </summary>
        public static bool Activate(IList<Project> projects, object finder)
        {
            return Activate(projects, finder as IClassFinder, finder as IStructFinder);
        }

        /// <summary>
        /// Open code editor window and set the cursor at the start of particular class or structure.
        /// Returns 'true' if editor opened, otherwise 'false'.
        /// </summary>
        public static bool Activate(IList<Project> projects, IClassFinder classFinder, IStructFinder structFinder)
        {
            // nothing to do:
            if ((classFinder == null && structFinder == null) || projects == null || projects.Count == 0)
                return false;

            ProjectItems pi;
            int i;

            foreach (Project prj in projects)
            {
                pi = prj.ProjectItems;

                for (i = 1; i < pi.Count; i++)
                {
                    ProjectItem f = pi.Item(i);

                    if (InternalEnumeration(f, classFinder, structFinder))
                        return true;
                }
            }

            return false;
        }

        private static bool ValidateItem(ProjectItem item, IClassFinder classFinder, IStructFinder structFinder)
        {
            IList<CodeNamespace> codeNamespaces;
            IList<CodeClass> codeClasses;
            IList<CodeStruct> codeStructs;
            FileCodeModel fcm;

            fcm = item.FileCodeModel;

            if (fcm != null)
                codeNamespaces = EditorHelper.GetList<CodeNamespace>(fcm.CodeElements, vsCMElement.vsCMElementNamespace);
            else
                codeNamespaces = null;

            if (classFinder != null && fcm != null)
                codeClasses = EditorHelper.GetList<CodeClass>(fcm.CodeElements, vsCMElement.vsCMElementClass);
            else
                codeClasses = null;
            if (structFinder != null && fcm != null)
                codeStructs = EditorHelper.GetList<CodeStruct>(fcm.CodeElements, vsCMElement.vsCMElementStruct);
            else
                codeStructs = null;

            if (InternalEnumeration(item, codeNamespaces, codeClasses, codeStructs, classFinder, structFinder))
                return true;

            return false;
        }

        private static bool InternalEnumeration(ProjectItem item, IClassFinder classFinder, IStructFinder structFinder)
        {
            ProjectItems subItems;

            if (item != null)
            {
                if (ValidateItem(item, classFinder, structFinder))
                    return true;

                subItems = item.ProjectItems;

                if (subItems != null && subItems.Count > 0)
                {
                    foreach (ProjectItem x in subItems)
                        if (ValidateItem(x, classFinder, structFinder))
                            return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Recursive enumeration of all solution items.
        /// </summary>
        private static bool InternalEnumeration(ProjectItem item, IList<CodeNamespace> codeNamespaces, IList<CodeClass> codeClasses, IList<CodeStruct> codeStructs, IClassFinder classFinder, IStructFinder structFinder)
        {
            IList<CodeNamespace> interNamespaces;
            IList<CodeClass> interClasses;
            IList<CodeStruct> interStructs;

            // enumerate through all namespaces:
            if (codeNamespaces != null)
            {
                foreach (CodeNamespace n in codeNamespaces)
                {
                    interNamespaces = EditorHelper.GetList<CodeNamespace>(n.Members, vsCMElement.vsCMElementNamespace);
                    if (classFinder != null)
                        interClasses = EditorHelper.GetList<CodeClass>(n.Members, vsCMElement.vsCMElementClass);
                    else
                        interClasses = null;

                    if (structFinder != null)
                        interStructs = EditorHelper.GetList<CodeStruct>(n.Members, vsCMElement.vsCMElementStruct);
                    else
                        interStructs = null;

                    if (InternalEnumeration(item, interNamespaces, interClasses, interStructs, classFinder, structFinder))
                        return true;
                }
            }

            // enumerate classes:
            if (codeClasses != null && classFinder != null)
            {
                foreach (CodeClass c in codeClasses)
                {
                    if (classFinder.MatchCriteria(c))
                    {
                        Window w = item.Open(Constants.vsViewKindTextView);
                        TextSelection sel = w.Selection as TextSelection;

                        classFinder.Activate(item, c, w, sel);
                        return true;
                    }

                    interClasses = EditorHelper.GetList<CodeClass>(c.Members, vsCMElement.vsCMElementClass);

                    if (codeStructs != null)
                        interStructs = EditorHelper.GetList<CodeStruct>(c.Members, vsCMElement.vsCMElementStruct);
                    else
                        interStructs = null;

                    if (InternalEnumeration(item, null, interClasses, interStructs, classFinder, structFinder))
                        return true;
                }
            }

            // enumerate structures:
            if (codeStructs != null && structFinder != null)
            {
                foreach (CodeStruct s in codeStructs)
                {
                    if (structFinder.MatchCriteria(s))
                    {
                        Window w = item.Open(Constants.vsViewKindTextView);
                        TextSelection sel = w.Selection as TextSelection;

                        structFinder.Activate(item, s, w, sel);
                        return true;
                    }

                    if (classFinder != null)
                        interClasses = EditorHelper.GetList<CodeClass>(s.Members, vsCMElement.vsCMElementClass);
                    else
                        interClasses = null;
                    interStructs = EditorHelper.GetList<CodeStruct>(s.Members, vsCMElement.vsCMElementStruct);

                    if (InternalEnumeration(item, null, interClasses, interStructs, classFinder, structFinder))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates the proper finder based on the message and code-jump style.
        /// </summary>
        public static ClassFinder GetFinder(string message, DbgViewCodeJumpStyle style)
        {
            return GetFinder(message, style, true);
        }

        /// <summary>
        /// Creates the proper finder based on the message and code-jump style.
        /// </summary>
        public static ClassFinder GetFinder(string message, DbgViewCodeJumpStyle style, bool autoExecute)
        {
            string className = null;
            string methodName = null;
            string[] args;

            if(string.IsNullOrEmpty(message))
                return null;

            // generate array of 6 elements...
            args = message.Split(DebugViewData.SplitChars, 6);

            // obtain the class/method name:
            switch (style)
            {
                case DbgViewCodeJumpStyle.Class_1:
                    if (args.Length > 0)
                        className = args[0];
                    break;
                case DbgViewCodeJumpStyle.Class_2:
                    if (args.Length > 1)
                        className = args[1];
                    break;
                case DbgViewCodeJumpStyle.Class_3:
                    if (args.Length > 2)
                        className = args[2];
                    break;
                case DbgViewCodeJumpStyle.Class_4:
                    if (args.Length > 3)
                        className = args[3];
                    break;
                case DbgViewCodeJumpStyle.Class_5:
                    if (args.Length > 4)
                        className = args[4];
                    break;

                case DbgViewCodeJumpStyle.Class_12:
                    if (args.Length > 0)
                        className = args[0];
                    if (args.Length > 1)
                        methodName = args[1];
                    break;
                case DbgViewCodeJumpStyle.Class_23:
                    if (args.Length > 1)
                        className = args[1];
                    if (args.Length > 2)
                        methodName = args[2];
                    break;
                case DbgViewCodeJumpStyle.Class_34:
                    if (args.Length > 2)
                        className = args[2];
                    if (args.Length > 3)
                        methodName = args[3];
                    break;
                case DbgViewCodeJumpStyle.Class_45:
                    if (args.Length > 3)
                        className = args[3];
                    if (args.Length > 4)
                        methodName = args[4];
                    break;
                case DbgViewCodeJumpStyle.Class_56:
                    if (args.Length > 4)
                        className = args[4];
                    if (args.Length > 5)
                        methodName = args[5];
                    break;

                default:
                    return null;
            }

            return new ClassFinder(className, methodName, autoExecute);
        }
    }
}
