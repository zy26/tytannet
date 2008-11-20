using System;
using System.ComponentModel.Design;
using System.Threading;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Forms;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Data;
using System.Diagnostics;

namespace Pretorianie.Tytan.Actions.Misc
{
    /// <summary>
    /// Class defining actions for 'About' popup menu of TytanNET AddIn.
    /// </summary>
    public class AboutBoxAction : IPackageAction
    {
        private const string PersistantStorageName = "AboutInfo";
        private const int SleepTimeBeforeUpdateCheck = 5 * 60 * 1000; // wait 5 minutes before first check of new version
        private const int PeriodBeforeUpdateCheck = 7; // perform a version check each week

        private IPackageEnvironment parent;
        private AboutBoxTipsForm dlgTips;
        private AboutBoxUpdateForm dlgUpdate;
        private AboutBoxNewVersionForm dlgNewVersion;
        private AboutBoxForm dlgAbout;
        private TipsProvider tips;
        private DateTime lastUpdateCheck = DateTime.MinValue;

        #region State Management

        private void CheckVersion()
        {
            PersistentStorageData data = PersistentStorageHelper.Load(PersistantStorageName);
            bool executeCheck = false;

            if (data != null && data.Count > 0)
            {
                // get the last update date from the registry:
                lastUpdateCheck = data.GetDateTime("LastUpdateDate");

                // check if update-check was performed at least one week ago:
                if (lastUpdateCheck.AddDays(PeriodBeforeUpdateCheck) < DateTime.Today)
                {
                    executeCheck = true;
                }
                else
                {
                    // prevent updating the date inside the registry:
                    lastUpdateCheck = DateTime.MinValue;
                }
            }
            else
            {
                executeCheck = true;
            }

            if (executeCheck)
            {
                // start asynchronously new thread that will perform check and update the registry:
                Thread threadCheck = new Thread(PerformUpdateCheck);
                threadCheck.Start();
            }
        }

        /// <summary>
        /// Store inside the persistent storage, when the last update took place.
        /// </summary>
        private void StoreCheckState()
        {
            if (lastUpdateCheck == DateTime.MinValue)
                return;

            PersistentStorageData data = new PersistentStorageData(PersistantStorageName);

            data.Add("LastUpdateDate", lastUpdateCheck.ToShortDateString());

            // store:
            PersistentStorageHelper.Save(data);
        }

        private void PerformUpdateCheck()
        {
            try
            {
                // sleep some time, to give the Visual Studio possibility to load correctly:
                Thread.Sleep(SleepTimeBeforeUpdateCheck);
                VersionHelper.CheckVersion(ReceivedVersionInfo);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }

        private void ReceivedVersionInfo(Version currentVersion, Version newVersion, string navigationURL)
        {
            try
            {
                if (currentVersion < newVersion)
                {
                    if (dlgNewVersion == null)
                        dlgNewVersion = new AboutBoxNewVersionForm();

                    // and update the dialog with proper content:
                    dlgNewVersion.SetUI(newVersion.ToString(VersionHelper.NumberOfDigits), navigationURL);
                    dlgNewVersion.Show();
                }

                lastUpdateCheck = DateTime.Today;
                StoreCheckState();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }

        #endregion

        /// <summary>
        /// Gets the unique ID of the action.
        /// </summary>
        public int ID
        {
            get { return PackageCmdIDList.toolAction_AbouxBoxPopup; }
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
            get { return null; }
            set { }
        }

        /// <summary>
        /// Performs initialization of this action and
        /// also registers all the UI elements required by the action, e.g.: menus / menu groups / toolbars.
        /// </summary>
        public void Initialize(IPackageEnvironment env, IMenuCreator mc)
        {
            MenuCommand tipsMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_AbouxBoxPopup, ExecuteTips);
            MenuCommand checkMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_AbouxBoxCheckUpdate, ExecuteCheckUpdate);
            MenuCommand bugMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_AbouxBoxSubmitBug, ExecuteBug);
            MenuCommand featureMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_AbouxBoxRequestFeature, ExecuteFeature);
            MenuCommand visitMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_AbouxBoxVisit, ExecuteVisit);
            MenuCommand aboutMenu = ObjectFactory.CreateCommand(GuidList.guidCmdSet, PackageCmdIDList.toolAction_AbouxBoxInfo, ExecuteAboutBox);

            parent = env;

            // -------------------------------------------------------
            mc.AddCommand(tipsMenu, "TipsAndTricks", "&Tips && tricks...", 9013, null, null, false);
            mc.AddCommand(checkMenu, "CheckForUpdate", "&Check for update...", 0, null, null, false);
            mc.AddCommand(bugMenu, "SubmitBug", "Submit &bug...", 0, null, null, false);
            mc.AddCommand(featureMenu, "SubmitFeature", "Submit &feature request...", 0, null, null, false);
            mc.AddCommand(visitMenu, "VisitHomepage", "Visit &homepage", 9014, null, null, false);
            mc.AddCommand(aboutMenu, "AboutBox", "&About...", 0, null, null, false);
            mc.Customizator.AddAboutItem(tipsMenu, false, 1, null);
            mc.Customizator.AddAboutItem(checkMenu, false, 2, null);
            mc.Customizator.AddAboutItem(bugMenu, true, 3, null);
            mc.Customizator.AddAboutItem(featureMenu, false, 4, null);
            mc.Customizator.AddAboutItem(visitMenu, false, 5, null);
            mc.Customizator.AddAboutItem(aboutMenu, true, 6, null);

            CheckVersion();
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
        }

        /// <summary>
        /// Actions performed when "Tips & Tricks" menu clicked.
        /// </summary>
        private void ExecuteTips(object sender, EventArgs e)
        {
            if (dlgTips == null)
            {
                tips = new TipsProvider();
                dlgTips = new AboutBoxTipsForm(tips);
            }

            dlgTips.ShowDialog();
        }

        /// <summary>
        /// Action performed when "Check for update" menu clicked.
        /// </summary>
        private void ExecuteCheckUpdate(object sender, EventArgs e)
        {
            CreateUpdateDialog();
            dlgUpdate.ShowDialog();
        }

        /// <summary>
        /// Initializes the update dialog.
        /// </summary>
        private void CreateUpdateDialog()
        {
            if (dlgUpdate == null)
            {
                dlgUpdate = new AboutBoxUpdateForm();
                dlgUpdate.SetupUI(parent.Info.Icon);
            }
        }

        /// <summary>
        /// Actions performed when "Submit B-U-G" menu clicked.
        /// </summary>
        private static void ExecuteBug(object sender, EventArgs e)
        {
            CallHelper.OpenBrowser(SharedStrings.About_Bug);
        }

        /// <summary>
        /// Actions performed when "Submit feature request" menu clicked.
        /// </summary>
        private static void ExecuteFeature(object sender, EventArgs e)
        {
            CallHelper.OpenBrowser(SharedStrings.About_Feature);
        }

        /// <summary>
        /// Actions performed when "Visit homepage" menu clicked.
        /// </summary>
        private static void ExecuteVisit(object sender, EventArgs e)
        {
            CallHelper.OpenBrowser(SharedStrings.About_Hopemage);
        }

        /// <summary>
        /// Action performed when "About..." menu clicked.
        /// </summary>
        private void ExecuteAboutBox(object sender, EventArgs e)
        {
            if (dlgAbout == null)
                dlgAbout = new AboutBoxForm();

            dlgAbout.SetUI(parent);
            dlgAbout.ShowDialog();
        }

    }
}
