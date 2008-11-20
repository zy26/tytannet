using EnvDTE;
using System.CodeDom;

namespace Pretorianie.Tytan.Core.Data.Refactoring
{
    /// <summary>
    /// Class describing named <c>CodeVariable</c> element.
    /// </summary>
    public class CodeVariableNamedElement : CodeNamedElement
    {
        private readonly CodeVariable variable;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CodeVariableNamedElement(CodeVariable variable, bool isDisabled, string paramName)
            : base(isDisabled, paramName)
        {
            this.variable = variable;
        }

        /// <summary>
        /// Gets the name of the referenced type.
        /// </summary>
        public override string ReferecedTypeName
        {
            get { return variable.Type.AsString; }
        }

        /// <summary>
        /// Gets the indication if given element is static.
        /// </summary>
        public override bool IsStatic
        {
            get { return variable.IsShared; }
        }

        /// <summary>
        /// Gets the name of special element.
        /// </summary>
        public override string GetName(ElementNames type)
        {
            switch (type)
            {
                case ElementNames.DefaultValid:
                case ElementNames.AsVariable:
                    return variable.Name;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the <c>CodeExpression</c> element that can reference current element.
        /// </summary>
        public override CodeExpression GetReferenceExpression(string className)
        {
            if (IsStatic)
                return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(className), Name);
            return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), Name);
        }
    }
}
