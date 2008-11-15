using System;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Events;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using EnvDTE;

namespace Pretorianie.Tytan.Actions.Misc
{
    /// <summary>
    /// Action that monitors solution events and prompts the user for confirmation, each time solution is about to close.
    /// </summary>
    public class VisualStudioCloseQuestion : IPackageAction
    {
        private SolutionEventsListener solutionListener;
        private PersistentStorageData config;

        #region Config Parameters

        /// <summary>
        /// Name of the configuration.
        /// </summary>
        public const string ConfigurationName = "CloseSolution";
        /// <summary>
        /// Name of the "Ask" configuration parameter.
        /// </summary>
        public const string Config_Prompt = "PromptForClose";

        #endregion

        #region Implementation of IPackageAction

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolAction_QuitQuestion; }
        }

        /// <summary>
        /// Gets the current valid configuration for the action. In case of
        /// null-value no settings are actually needed at all.
        /// 
        /// Set is executed at runtime when the configuration for
        /// given action is updated via external module (i.e. Tools->Options).
        /// </summary>
        public PersistentStorageData Configuration
        {
            get { return config; }
            set { config = value; }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCreator mc)
        {
            solutionListener = new SolutionEventsListener(env.DTE);
            solutionListener.SolutionQueryClose += SolutionEvents_SolutionQueryClose;

            config = ObjectFactory.LoadConfiguration(ConfigurationName);
        }

        void SolutionEvents_SolutionQueryClose(object sender, Solution s, ref bool bCancel)
        {
            if (config == null || config.GetUInt(Config_Prompt, 0) > 0)
            {
                if (MessageBox.Show(SharedStrings.SolutionClose_Question, SharedStrings.SolutionClose_DialogTitle,
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    bCancel = true;
            }
        }

        /// <summary>
        /// Invokes proper processing assigned to current action.
        /// </summary>
        public void Execute(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Executed on Visual Studio exit.
        /// All non-managed resources should be released here.
        /// </summary>
        public void Destroy()
        {
            if (solutionListener != null)
            {
                solutionListener.Dispose();
                solutionListener = null;
            }
        }

        #endregion
    }
}
