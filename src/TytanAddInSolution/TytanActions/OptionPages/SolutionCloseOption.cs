using Pretorianie.Tytan.Actions.Misc;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.OptionPages
{
    public partial class SolutionCloseOption : Core.BaseForms.BaseOptionPage
    {
        private PersistentStorageData config;

        public SolutionCloseOption()
        {
            InitializeComponent();
        }

        protected override System.Type ActionType
        {
            get { return typeof(VisualStudioCloseQuestion); }
        }

        protected override void ConfigurationPresent()
        {
            config = ObjectFactory.LoadConfiguration(VisualStudioCloseQuestion.ConfigurationName);

            // present configuration on the screen:
            promptOnClosing.Checked = config.GetUInt(VisualStudioCloseQuestion.Config_Prompt, 1) > 0;
        }

        protected override void ConfigurationUpdate(out PersistentStorageData actionConfig)
        {
            // update configuration:
            config.Add(VisualStudioCloseQuestion.Config_Prompt, (uint) (promptOnClosing.Checked ? 1 : 0));

            actionConfig = config;
        }
    }
}