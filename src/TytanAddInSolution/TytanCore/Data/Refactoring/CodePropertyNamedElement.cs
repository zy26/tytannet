using EnvDTE;
using System.CodeDom;

namespace Pretorianie.Tytan.Core.Data.Refactoring
{
    /// <summary>
    /// Class describing named <c>CodeProperty</c> element.
    /// </summary>
    public class CodePropertyNamedElement : CodeNamedElement
    {
        private readonly CodeProperty property;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CodePropertyNamedElement(CodeProperty property, bool isDisabled, string paramName)
            : base(isDisabled, paramName)
        {
            this.property = property;
        }

        /// <summary>
        /// Gets the name of the referenced type.
        /// </summary>
        public override string ReferecedTypeName
        {
            get { return property.Getter != null ? property.Getter.Type.AsString : null; }
        }

        /// <summary>
        /// Gets the indication if given element is static.
        /// </summary>
        public override bool IsStatic
        {
            get
            {
                return (property.Getter == null || property.Getter.IsShared)
                       && (property.Setter == null || property.Setter.IsShared);
            }
        }

        /// <summary>
        /// Gets the name of special element.
        /// </summary>
        public override string GetName(ElementNames type)
        {
            switch (type)
            {
                case ElementNames.DefaultValid:
                case ElementNames.AsProperty:
                    return property.Name;

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
                return new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(className), Name);
            return new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), Name);
        }
    }
}
