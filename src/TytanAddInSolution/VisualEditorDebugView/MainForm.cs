using System.Windows.Forms;

namespace VisualEditor.DebugView
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            debugViewTool1.ServiceEnabled = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            debugViewTool1.ServiceEnabled = false;
        }
    }
}