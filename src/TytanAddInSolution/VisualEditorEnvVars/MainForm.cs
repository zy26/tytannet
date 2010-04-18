using System.Windows.Forms;

namespace VisualEditor.Environment
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save the history values for edited variables.
        /// </summary>
        public void SaveHistory()
        {
            environmentVarsTool1.SaveHistory();
        }
    }
}