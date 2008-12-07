using System.Collections.Generic;
using Pretorianie.Tytan.Core.DbgView.Sources;

namespace Pretorianie.Tytan.Forms
{
    public partial class DebugViewCloseForm : Core.BaseForms.BasePackageForm
    {
        public DebugViewCloseForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets given data to UI.
        /// </summary>
        public void InitializeUI(IList<IDbgSource> list)
        {
            sources.Items.Clear();

            if (list != null)
                foreach (IDbgSource s in list)
                    sources.Items.Add(s.Description);
        }
    }
}
