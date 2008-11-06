using System;
using Pretorianie.Tytan.Actions.Misc;

namespace Pretorianie.Tytan.OptionPages
{
    public partial class ReferenceProjectOption : Core.BaseForms.BaseOptionPage
    {
        public ReferenceProjectOption()
        {
            InitializeComponent();
        }

        protected override Type ActionType
        {
            get { return typeof (ReferenceProjectAction); }
        }

        private void bttReload_Click(object sender, EventArgs e)
        {
            ExecuteAction(null);
        }
    }
}

