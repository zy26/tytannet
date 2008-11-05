using System.Windows.Forms;

namespace VisualEditor.Registry
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            registryViewTool1.StoreState();
        }
    }
}