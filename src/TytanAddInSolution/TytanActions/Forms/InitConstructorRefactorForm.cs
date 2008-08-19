using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using Pretorianie.Tytan.Core.BaseForms;

namespace Pretorianie.Tytan.Forms
{
    public partial class InitConstructorRefactorForm : BasePackageForm
    {
        private IList<CodeVariable> storedVars;

        public InitConstructorRefactorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the interface with the given number of variables, their names and proposal for accompanying param name.
        /// </summary>
        public void InitInterface(IList<CodeVariable> vars, IList<CodeVariable> toDisable, IList<string> paramNames)
        {
            int i = 0;

            dataVars.Rows.Clear();
            if (vars != null)
            {
                storedVars = vars;
                foreach (CodeVariable v in vars)
                {
                    dataVars.Rows.Add((toDisable != null && toDisable.Contains(v) ? "false" : "true"), v.Name, paramNames[i]);
                    i++;
                }
            }
        }

        /// <summary>
        /// Gets the variables and params selected by user and their modified names.
        /// </summary>
        public bool ReadInterface(out IList<CodeVariable> vars, out IList<string> paramNames)
        {
            int i = 0;

            vars = new List<CodeVariable>();
            paramNames = new List<string>();

            foreach (DataGridViewRow r in dataVars.Rows)
            {
                if (string.Compare(r.Cells[0].Value as string, bool.TrueString, true) == 0)
                {
                    vars.Add(storedVars[i]);
                    paramNames.Add(r.Cells[2].Value as string);
                }
                i++;
            }

            // deletes empty collections:
            if (vars.Count == 0)
            {
                vars = null;
                paramNames = null;
                return false;
            }

            return true;
        }
    }
}

