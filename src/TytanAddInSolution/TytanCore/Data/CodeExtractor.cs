using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using EnvDTE;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Class extracting information from specified file.
    /// </summary>
    public abstract class CodeExtractor
    {
        private const string Operators = "./:*-_<>[]();";
        private const string Identifiers = "_@";
        private readonly static char[] TemplateOperators = new char[] {'<', '>', '(', ')', '[', ']'};

        /// <summary>
        /// Current editor cursor location.
        /// </summary>
        protected CodeEditPoint editPoint;
        private IList<string> namespaces;
        private string identifier;

        /// <summary>
        /// Init constructor of CodeExtractor.
        /// </summary>
        protected CodeExtractor(CodeEditPoint editPoint)
        {
            if (editPoint == null)
                throw new ArgumentNullException("editPoint");

            this.editPoint = editPoint;
        }

        /// <summary>
        /// Gets the locaiton inside active document.
        /// </summary>
        public EditPoint EditPoint
        {
            get { return editPoint.EditPoint; }
        }

        /// <summary>
        /// Gets the list of namespaces valid for current cursor location.
        /// </summary>
        public IList<string> Namespaces
        {
            get
            {
                if (namespaces == null)
                    namespaces = BrowseCurrentNamespaceForImports();

                return namespaces;
            }
        }

        /// <summary>
        /// Gets the identifier from cursor.
        /// </summary>
        public string CurrentIdentifier
        {
            get
            {
                if (string.IsNullOrEmpty(identifier))
                    identifier = GetCurrentIdentifier(true);

                return identifier;
            }
        }

        /// <summary>
        /// Searches for import/using directives inside the current namespace.
        /// </summary>
        private IList<string> BrowseCurrentNamespaceForImports()
        {
            IList<string> ns = new List<string>();
            CodeClass codeClass;
            CodeStruct codeStruct;

            // get current namespace:
            CodeNamespace cn = editPoint.GetCurrentCodeElement<CodeNamespace>(vsCMElement.vsCMElementNamespace);

            // then browse all the child namespaces searching for 'using' or 'import' or whatewer elements:
            if (ImportNamespace != null)
                foreach (CodeElement element in editPoint.CodeModel.CodeElements)
                    InternalGetNamespaces(element, ns);

            // and store its name:
            if (cn != null)
                ns.Add(cn.FullName);

            // add the full name of current class/structure:
            if (EditorHelper.GetParent(editPoint, out codeClass, out codeStruct))
            {
                if (codeClass != null)
                    ns.Add(codeClass.FullName);
                else
                    ns.Add(codeStruct.FullName);
            }

            // and return parsing result:
            return ns;
        }

        /// <summary>
        /// Calculates the list of namespaces for current location.
        /// </summary>
        protected void InternalGetNamespaces(CodeElement element, IList<string> resultNamespaces)
        {
            if (element.Kind == vsCMElement.vsCMElementImportStmt)
            {
                EditPoint startEdit = element.StartPoint.CreateEditPoint();
                string text = startEdit.GetText(element.EndPoint);

                // check if specified namespace name can be extracted from element:
                Match match = ImportNamespace.Match(text);
                if (match.Groups.Count > 2)
                    resultNamespaces.Add(match.Groups[1].Value.Replace(" ", string.Empty).Replace("\t", string.Empty));
            }

            // check other namespaces for 'import' directives:
            if (element.Kind == vsCMElement.vsCMElementNamespace)
            {
                foreach (CodeElement children in element.Children)
                    InternalGetNamespaces(children, resultNamespaces);
            }
        }

        /// <summary>
        /// Gets the name of identifier from current location.
        /// </summary>
        public string GetCurrentIdentifier(bool forceNamespaceSeparator)
        {
            int offsetStart = editPoint.EditPoint.LineCharOffset - 1;


            // check if parsing starts inside a comment
            // and if it is 'true' then stop further steps:
            CodeLineNavigator navigator = new CodeLineNavigator(this, editPoint.EditPoint.Line);
            int index = GetCommentIndexStart(navigator.FirstLine);
            if (string.IsNullOrEmpty(navigator.FirstLine) || (index >= 0 && index < offsetStart))
                return null;

            CodeWordNavigator word = new CodeWordNavigator(navigator, offsetStart);
            StringBuilder result = new StringBuilder();
            bool isOper;
            bool isPrevOper;
            bool isFirstOper;
            string w;

            // get all the language tokens on the right side of cursor:
            if ((w = word.NextWord(out isPrevOper)) != null)
            {
                if (isPrevOper)
                {
                    if (!IsValidSeparator(w))
                        return result.ToString();

                    if (forceNamespaceSeparator && !IsTemplateOperator(w))
                        result.Append(NamespaceSeparator);
                    else result.Append(w);
                }
                else
                    result.Append(w);
                isFirstOper = isPrevOper;

                while ((w = word.NextWord(out isOper)) != null &&
                    (isOper != isPrevOper || IsTemplateOperator(w) || !isOper && CanSkipNamespaceSeparator(w)))
                {
                    // is special operator that looks like an identifier? (VB specific - '_')
                    if (!isOper && CanSkipNamespaceSeparator(w))
                        isOper = true;

                    if (isOper)
                    {
                        if (IsTerminalInstructionSeparator(w))
                            break;
                        if (!CanSkipNamespaceSeparator(w))
                        {
                            if (!IsValidSeparator(w))
                                return result.ToString();

                            if (forceNamespaceSeparator && !IsTemplateOperator(w))
                                result.Append(NamespaceSeparator);
                            else result.Append(w);
                            isPrevOper = true;
                        }
                    }
                    else
                    {
                        result.Append(w);
                        isPrevOper = false;
                    }
                }

                // reset to the position, where read was started:
                word.ResetForPrevious();

                // read all the words on left:
                isPrevOper = isFirstOper;
                while ((w = word.PreviousWord(out isOper)) != null &&
                    (isOper != isPrevOper || IsTemplateOperator(w) || (!isOper && CanSkipNamespaceSeparator(w))))
                {
                    // is special operator that looks like an identifier? (VB specific - '_')
                    if (!isOper && CanSkipNamespaceSeparator(w))
                        isOper = true;

                    if (isOper)
                    {
                        if (IsTerminalInstructionSeparator(w))
                            break;
                        if (!CanSkipNamespaceSeparator(w))
                        {
                            if (!IsValidSeparator(w))
                                return result.ToString();

                            if (forceNamespaceSeparator)
                                result.Insert(0, NamespaceSeparator);
                            else result.Insert(0, w);
                            isPrevOper = true;
                        }
                    }
                    else
                    {
                        result.Insert(0, w);
                        isPrevOper = false;
                    }
                }

            }

            return result.ToString();
        }

        /// <summary>
        /// Checks if given character is a part of an identifier.
        /// </summary>
        public bool IsIdentifierChar(char c)
        {
            return char.IsLetterOrDigit(c) || Identifiers.IndexOf(c) >= 0;
        }

        /// <summary>
        /// Checks if given character is a part of operator.
        /// </summary>
        public bool IsOperatorChar(char c)
        {
            return Operators.IndexOf(c) >= 0;
        }

        public bool IsTemplateOperator(string separator)
        {
            return separator.IndexOfAny(TemplateOperators) >= 0;
        }

        /// <summary>
        /// Gets the type info just by recursive lookup of specified name inside all existing namespaces.
        /// </summary>
        public CodeType GetTypeInfo(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            name = name.Trim('.', ':', '/');
            if (string.IsNullOrEmpty(name))
                return null;

            // is it alread a full-name ?
            CodeType t = editPoint.GetTypeInfo(name);
            if (t != null)
                return t;

            // visit all namespaces and check
            // if any of them contain the type definition:
            IList<string> ns = Namespaces;

            if (ns == null || ns.Count == 0)
                return null;

            for (int i = ns.Count - 1; i >= 0; i--)
            {
                t = VisitNamespace(ns[i], name);

                if (t != null)
                    return t;
            }

            // not found anywhere:
            return null;
        }

        /// <summary>
        /// Check all combinations of namespace names to localize given type.
        /// </summary>
        private CodeType VisitNamespace(string ns, string typeName)
        {
            string nsSeparator = NamespaceSeparator;

            while (!string.IsNullOrEmpty(ns))
            {
                string fullName = string.Format("{0}{1}{2}", ns, nsSeparator, typeName);
                CodeType t = editPoint.GetTypeInfo(fullName);

                if (t != null)
                    return t;

                // remove the last part of the namespace:
                int i = ns.LastIndexOf(nsSeparator);
                if (i < 0)
                    ns = null;
                else
                    ns = ns.Substring(0, i);
            }

            // all combinations are invalid:
            return null;
        }

        #region Abstract Functions / Properties

        /// <summary>
        /// Gets the regular expression to extract namespace's name from <c>CodeElement</c>.
        /// </summary>
        public abstract Regex ImportNamespace
        { get;  }

        /// <summary>
        /// Gets the operator that separates elements of namespaces.
        /// </summary>
        public abstract string NamespaceSeparator
        { get; }

        /// <summary>
        /// Gets the index of comment in given line.
        /// </summary>
        public abstract int GetCommentIndexStart(string text);

        /// <summary>
        /// Gets the index of comment in given line.
        /// </summary>
        public abstract int GetCommentIndexEnd(string text);

        /// <summary>
        /// Checks if given separator is a valid language-specific separator.
        /// </summary>
        public virtual bool IsValidSeparator(string separator)
        {
            return separator == NamespaceSeparator || IsTemplateOperator(separator);
        }

        /// <summary>
        /// Checks if given separator is a terminal between instructions.
        /// </summary>
        public virtual bool IsTerminalInstructionSeparator(string separator)
        {
            return false;
        }

        /// <summary>
        /// Checks if given separator can be skipped while reading namespace of identifier.
        /// </summary>
        public virtual bool CanSkipNamespaceSeparator(string separator)
        {
            return false;
        }

        #endregion
    }
}
