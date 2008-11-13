using System;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Core.BaseForms
{
    public partial class BaseOptionPage : UserControl, IDTToolsOptionsPage
    {
        private static IPackageConfigUpdater configUpdater;
        private DTE2 appObject;

        public BaseOptionPage()
        {
            InitializeComponent();
        }

        #region Implementation of IDTToolsOptionsPage

        public void OnAfterCreated(DTE appObject)
        {
            this.appObject = appObject as DTE2;
            ConfigurationPresent();
        }

        public virtual void GetProperties(ref object propertiesObject)
        {
        }

        public void OnOK()
        {
            PersistentStorageData config;

            ConfigurationUpdate(out config);
            UpdateActionConfig(configUpdater, ActionType, config);
        }

        public virtual void OnCancel()
        {
        }

        public virtual void OnHelp()
        {
        }

        /// <summary>
        /// Sets the configuration object for given action.
        /// </summary>
        private static void UpdateActionConfig(IPackageConfigUpdater cu, Type actionType, PersistentStorageData config)
        {
            if (cu != null && config != null && actionType != null)
            {
                cu.UpdateConfiguration(actionType, config);
                PersistentStorageHelper.Save(config);
            }
        }

        /// <summary>
        /// Executes method on given associated action.
        /// </summary>
        protected void ExecuteAction(EventArgs e)
        {
            ExecuteAction(configUpdater, ActionType, e);
        }

        private void ExecuteAction(IPackageConfigUpdater cu, Type actionType, EventArgs e)
        {
            if(cu != null && actionType != null)
                cu.UpdateExecute(actionType, e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the application object.
        /// </summary>
        public DTE2 DTE
        {
            get { return appObject; }
        }

        /// <summary>
        /// Gets or sets the handle to object providing access to specified action configuration.
        /// </summary>
        public static IPackageConfigUpdater ConfigProvider
        {
            get { return configUpdater; }
            set { configUpdater = value; }
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Gets the type of associated action.
        /// </summary>
        protected virtual Type ActionType
        {
            get { return null; }
        }

        /// <summary>
        /// Reads the configuration items and populates them over the interface.
        /// </summary>
        protected virtual void ConfigurationPresent()
        {
        }

        /// <summary>
        /// Writes the configuration items into persistent storage.
        /// To have a valid effect ActionType property must be overwritten and return
        /// correct type of an associated action.
        /// </summary>
        protected virtual void ConfigurationUpdate(out PersistentStorageData actionConfig)
        {
            actionConfig = null;
        }

        #endregion
    }
}
