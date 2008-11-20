using System.CodeDom;
namespace Pretorianie.Tytan.Core.Data.Refactoring
{
    /// <summary>
    /// Class describing named <c>CodeElement</c> from any language code-model.
    /// </summary>
    public abstract class CodeNamedElement
    {
        /// <summary>
        /// Defines types of names given element can provide.
        /// </summary>
        public enum ElementNames
        {
            DefaultValid,
            AsVariable,
            AsProperty
        }

        private bool isDisabled;
        private string paramName;

        /// <summary>
        /// Init constructor of CodeNamedElement.
        /// </summary>
        public CodeNamedElement(bool isDisabled, string paramName)
        {
            this.isDisabled = isDisabled;
            this.paramName = paramName;
        }

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        public string Name
        {
            get { return GetName(ElementNames.DefaultValid); }
        }

        /// <summary>
        /// Gets the name of the referenced type.
        /// </summary>
        public abstract string ReferecedTypeName
        { get; }

        /// <summary>
        /// Gets the indication if given element is static.
        /// </summary>
        public abstract bool IsStatic
        { get; }

        /// <summary>
        /// Gets the name of special element.
        /// </summary>
        public abstract string GetName(ElementNames type);

        /// <summary>
        /// Gets the <c>CodeExpression</c> element that can reference current element.
        /// </summary>
        public abstract CodeExpression GetReferenceExpression(string className);

        /// <summary>
        /// Gets or sets the state of an element.
        /// </summary>
        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        /// <summary>
        /// Gets the name of the parameter generated based on current <c>CodeElement</c>.
        /// </summary>
        public string ParameterName
        {
            get { return paramName; }
            set { paramName = value; }
        }
    }
}
