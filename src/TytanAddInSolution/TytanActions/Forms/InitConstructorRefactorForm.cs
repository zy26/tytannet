using System.Collections.Generic;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.BaseForms;
using Pretorianie.Tytan.Core.Data.Refactoring;

namespace Pretorianie.Tytan.Forms
{
    public partial class InitConstructorRefactorForm : BasePackageForm
    {
        private IList<CodeNamedElement> storedElemens;

        public InitConstructorRefactorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the interface with the given number of variables, their names and proposal for accompanying param name.
        /// </summary>
        public void InitInterface(IList<CodeNamedElement> codeElements)
        {
            dataVars.Rows.Clear();

            // insert variables and properties into grid on the GUI:
            if (codeElements != null)
            {
                storedElemens = codeElements;
                foreach (CodeNamedElement e in codeElements)
                    dataVars.Rows.Add(!e.IsDisabled, e.GetName(CodeNamedElement.ElementNames.AsVariable),
                        e.GetName(CodeNamedElement.ElementNames.AsProperty), e.ParameterName);
            }

            ActiveControl = bttOK;
        }

        /// <summary>
        /// Gets the variables and params selected by user and their modified names.
        /// </summary>
        public bool ReadInterface(out IList<CodeNamedElement> codeElements)
        {
            int i = 0;

            // and add only selected elements to the result list:
            codeElements = new List<CodeNamedElement>();
            foreach (DataGridViewRow r in dataVars.Rows)
            {
                if ((bool)r.Cells[0].Value)
                {
                    CodeNamedElement e = storedElemens[i];

                    e.ParameterName = r.Cells[3].Value as string;
                    e.IsDisabled = false;
                    codeElements.Add(e);
                }
                i++;
            }

            // deletes empty collection:
            if (codeElements.Count == 0)
            {
                codeElements = null;
                //return false;
            }

            return true;
        }
    }
}

