using System.Windows.Forms;
using Pretorianie.Tytan.Core.CustomAddIn;

namespace Pretorianie.Tytan.Tools
{
    public partial class TypeFinderTool : UserControl
    {
        public TypeFinderTool()
        {
            InitializeComponent();

            // store the reference of the created tool:
            CustomAddInManager.LastCreatedPackageTool = this;
        }
    }
}
