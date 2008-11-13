using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Pretorianie.Tytan.Core.BaseForms;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using System.Windows.Forms;
using System.Reflection;

namespace Pretorianie.Tytan.Core.CustomAddIn
{
    /// <summary>
    /// Class that manages the group of action belonging to the custom add-in.
    /// </summary>
    public class CustomAddInManager : IToolCreator, IPackageEnvironment, IPackageConfigUpdater
    {
        private readonly DTE2 appObject;
        private readonly AddIn addInInstance;
        private readonly CustomAddInActionDictionary actions;
        private readonly CustomAddInMenuManager menuManager;
        private readonly PackageAssist assist;
        private readonly Dictionary<IPackageToolWindow, Window> toolWindows = new Dictionary<IPackageToolWindow, Window>();

        private readonly Assembly mainAssembly;
        private Assembly satelliteAssembly;
        private ResourceManager satelliteResources;

        private bool menuAdded;

        private readonly Microsoft.VisualStudio.Shell.SelectionContainer selectionContainer;
        private readonly System.Collections.ArrayList selectedItems;
        private object[] selectedElements;
        private PackageInfo packageInfo;

        #region Static Properties

        private static Control lastCreatedPackageTool;

        /// <summary>
        /// Stores reference to the last created package tool window.
        /// </summary>
        public static Control LastCreatedPackageTool
        {
            get { return lastCreatedPackageTool; }
            set { lastCreatedPackageTool = value; }
        }

        #endregion

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CustomAddInManager(DTE2 applicationObject, AddIn addInInst, CustomAddInActionDictionary customActions, IMenuCustomizator menuCustomizator, Assembly assemblyForSatellites)
        {
            appObject = applicationObject;
            addInInstance = addInInst;
            actions = customActions;
            menuManager = new CustomAddInMenuManager(applicationObject, addInInst, menuCustomizator);
            assist = new PackageAssist(this);
            mainAssembly = assemblyForSatellites;

            selectionContainer = new Microsoft.VisualStudio.Shell.SelectionContainer();
            selectedItems = new System.Collections.ArrayList();
            selectedElements = new object[1];

            // all settings read/writes will be pefrom according current version of Visual Studio IDE:
            PersistentStorageHelper.Attach(applicationObject);
            BaseOptionPage.ConfigProvider = this;
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public CustomAddInManager(DTE2 applicationObject, AddIn addInInst, IMenuCustomizator menuCustomizator, Assembly assemblyForSatellites)
        {
            appObject = applicationObject;
            addInInstance = addInInst;
            actions = new CustomAddInActionDictionary();
            menuManager = new CustomAddInMenuManager(applicationObject, addInInst, menuCustomizator);
            assist = new PackageAssist(this);
            mainAssembly = assemblyForSatellites;

            selectionContainer = new Microsoft.VisualStudio.Shell.SelectionContainer();
            selectedItems = new System.Collections.ArrayList();
            selectedElements = new object[1];

            // all settings read/writes will be pefrom according current version of Visual Studio IDE:
            PersistentStorageHelper.Attach(applicationObject);
            BaseOptionPage.ConfigProvider = this;
        }

        /// <summary>
        /// Initialize all registered actions.
        /// Each action should have defined initialization of toolbars and menus.
        /// </summary>
        public void ApplicationInit(bool isSetupUI)
        {
            menuManager.IsSetupUI = isSetupUI;

            // on the first load of an add-in:
            if (!menuAdded && actions != null && actions.Count > 0)
            {
                menuAdded = true;
                SetupActions();
            }
        }

        /// <summary>
        /// Release used resources.
        /// </summary>
        public void ApplicationExit(bool deleteVsIdeElements)
        {
            actions.Dispose();
            assist.Dispose();
            menuManager.Dispose(deleteVsIdeElements);
            menuAdded = false;
        }

        /// <summary>
        /// Store the action for future usage.
        /// </summary>
        public void Add(IPackageAction action)
        {
            actions.Add(action);
        }

        /// <summary>
        /// Execute particular command by name.
        /// </summary>
        public void Execute(string commandName, ref bool handled)
        {
            try
            {
                MenuCommand menuItem = menuManager.GetByName(commandName);

                // execute action:
                if (menuItem != null && menuItem.Enabled && menuItem.Visible && menuItem.Supported)
                    menuItem.Invoke(this);

                if (menuItem != null)
                    handled = true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Query the named command status.
        /// </summary>
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                MenuCommand menuItem = menuManager.GetByName(commandName);

                // ask for status specified menu item:
                if (menuItem != null)
                    statusOption = (vsCommandStatus) menuItem.OleStatus;
            }
        }

        #region Private Setup

        /// <summary>
        /// Registers all the actions and their menu items.
        /// </summary>
        private void SetupActions()
        {
            try
            {
                actions.Initialize(this, menuManager);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                throw;
            }
        }

        #endregion

        #region IToolCreator Members

        string IToolCreator.ProgID
        {
            get { return addInInstance.ProgID; }
        }

        object IToolCreator.CreateToolWindow(IPackageToolWindow tool)
        {
            if (tool == null || tool.ControlType == null)
                return null;

            object objToolWindow = null;
            Type type = tool.ControlType;
            string guid = tool.Guid;

            // force the proper format of the GUID:
            if (!string.IsNullOrEmpty(guid) && guid[0] != '{' && guid[guid.Length - 1] != '}')
                guid = "{" + guid + "}";

            try
            {
                // create a new tool window, embedding the control inside it ActiveX host...
                Window2 window = ((Windows2) appObject.Windows).CreateToolWindow2(addInInstance, type.Assembly.Location,
                                                                                  type.FullName, tool.Caption, guid,
                                                                                  ref objToolWindow) as Window2;

                if(objToolWindow == null)
                {
                    objToolWindow = LastCreatedPackageTool;
                    lastCreatedPackageTool = null;
                }

                // validate created window-frame:
                if (window == null)
                    return null;

                // set proper icon retrieved from resources:
                if (tool.BitmapIndex > 0)
                {
                    Bitmap tabIcon = LoadImage(tool.BitmapIndex.ToString());

                    // set the tab-picture image:
                    ImageHelper.SetTabPicture(window, tabIcon);
                }

                // make the window a document inside the IDE:
                if (!tool.IsFloating)
                {
                    window.Linkable = false;
                    window.IsFloating = false;
                }

                // remember reference:
                tool.Window = window;
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
            }

            return objToolWindow;
        }



        #endregion

        #region IPackageEnvironment Members

        /// <summary>
        /// Gets the Visual Studio IDE application object.
        /// </summary>
        public DTE2 DTE
        {
            get { return appObject; }
        }

        /// <summary>
        /// Get the current version of Visual Studio IDE.
        /// </summary>
        public ShellVersions Version
        {
            get { return ShellHelper.GetVersion(DTE); }
        }

        /// <summary>
        /// Gets the current state of the Visual Studio IDE.
        /// </summary>
        public ShellModes Mode
        {
            get { return ShellHelper.GetMode(DTE); }
        }

        /// <summary>
        /// Gets the <see cref="CodeEditPoint"/> for current active document.
        /// </summary>
        public CodeEditPoint CurrentEditPoint
        {
            get
            {
                return ShellHelper.CreateEditPointForCurrentDocument(appObject);
            }
        }

        /// <summary>
        /// Gets the current info about given Visual Studio add-in.
        /// </summary>
        public PackageInfo Info
        {
            get
            {
                if (packageInfo == null)
                {
                    ValidateSatellites();

                    Icon i = satelliteResources.GetObject("AddIn_DefaultIcon") as Icon;

                    packageInfo = new PackageInfo(satelliteResources.GetString("AddIn_FriendlyName"),
                                                  satelliteResources.GetString("AddIn_Description"),
                                                  satelliteResources.GetString("AddIn_AboutBox"),
                                                  i != null ? i.ToBitmap() : null,
                                                  VersionHelper.CurrentVersion);
                }

                return packageInfo;
            }
        }

        /// <summary>
        /// Gets the instance of menu creator object.
        /// </summary>
        public IMenuCreator MenuCreator
        {
            get { return menuManager; }
        }

        /// <summary>
        /// Shows (or creates if needed) the specified tool window.
        /// </summary>
        public bool ShowToolWindow(IPackageToolWindow toolWindow)
        {
            Window t;
            Control c;

            if (!toolWindows.TryGetValue(toolWindow, out t) || t == null)
            {
                c = ((IToolCreator) this).CreateToolWindow(toolWindow) as Control;
                toolWindow.Control = c;
                if (c != null)
                {
                    c.Text = toolWindow.Caption;
                    toolWindow.Window.Visible = true;

                    // store for future usage:
                    toolWindows.Add(toolWindow, toolWindow.Window);
                    return true;
                }

                Trace.WriteLine("Error during creation of tool-window form.");
                return false;
            }

            t.Visible = true;
            return false;

            //ShellHelper.ShowToolWindow(toolWindow.ToolWindowPaneType, assist, false, false);
        }

        /// <summary>
        /// Load satellite modules.
        /// </summary>
        private void ValidateSatellites ()
        {
            // load proper satelite assembly:
            if (satelliteAssembly == null)
            {
                satelliteAssembly = mainAssembly.GetSatelliteAssembly(
                        System.Threading.Thread.CurrentThread.CurrentUICulture);

                satelliteResources = new ResourceManager("PublicResources", satelliteAssembly);
            }
        }

        /// <summary>
        /// Loads image from current package resources or satellite DLLs.
        /// </summary>
        /// <param name="name">Name of the image-resource to load.</param>
        public Bitmap LoadImage(string name)
        {
            try
            {
                ValidateSatellites();

                // load resources:
                if (satelliteResources != null)
                    return satelliteResources.GetObject(name) as Bitmap;

                return null;
            }
            catch (Exception ex)
            {
                Trace.Write(string.Format("Can not load image '{0}' from satellite DLLs.", name));
                Trace.Write(ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Sets given object as a source provide of 'Properties Window' inside Visual Studio IDE.
        /// </summary>
        /// <param name="currentWindow">Source window, that requests set.</param>
        /// <param name="sourceObject">Object, which properties will be edited.</param>
        public void SelectAtProperties(Window2 currentWindow, object sourceObject)
        {
            //IVsUIShell shell = parent.GetService(typeof(SVsUIShell)) as IVsUIShell;
            //Guid guidPropertyBrowser = new Guid(ToolWindowGuids.PropertyBrowser);
            //IVsWindowFrame frame = null;

            //if (shell != null)
            //    shell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guidPropertyBrowser, out frame);

            //if (frame != null)
            //    frame.ShowNoActivate();

            ITrackSelection trackSelection = GetService(typeof (STrackSelection)) as ITrackSelection;
            if (trackSelection != null)
            {
                selectedItems.Clear();
                if (sourceObject != null)
                    selectedItems.Add(sourceObject);

                selectionContainer.SelectedObjects = selectedItems;
                trackSelection.OnSelectChange(selectionContainer);
            }
            else
            {
                if (sourceObject != null && currentWindow != null)
                {
                    selectedElements[0] = sourceObject;
                    currentWindow.SetSelectionContainer(ref selectedElements);
                    currentWindow.Activate();
                }
            }
        }

        #endregion

        #region IServiceProvider Members

        ///<summary>
        ///Gets the service object of the specified type.
        ///</summary>
        ///<returns>
        ///A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.
        ///</returns>
        ///<param name="serviceType">An object that specifies the type of service object to get. </param><filterpriority>2</filterpriority>
        public object GetService(Type serviceType)
        {
            return ShellHelper.GetService(appObject as Microsoft.VisualStudio.OLE.Interop.IServiceProvider, serviceType);
        }

        #endregion

        #region Implementation of IPackageConfigProvider

        /// <summary>
        /// Sets the configuration description for given action.
        /// </summary>
        public void UpdateConfiguration(Type actionType, PersistentStorageData config)
        {
            IPackageAction action = actions.Get(actionType);

            if (action != null)
                action.Configuration = config;
        }

        /// <summary>
        /// Calls an update method related with given button on the configuration GUI.
        /// </summary>
        public void UpdateExecute(Type actionType, EventArgs e)
        {
            IPackageAction action = actions.Get(actionType);

            if(action != null)
                action.Execute(null, e);
        }

        #endregion
    }
}