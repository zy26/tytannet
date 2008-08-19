using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using EnvDTE;
using EnvDTE80;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Imaging;

namespace Pretorianie.Tytan.Core.CustomAddIn
{
    /// <summary>
    /// Class that manages the AddIn.
    /// </summary>
    public class CustomAddInManager : IToolCreator, IPackageEnvironment
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
        private CustomImageConverter imageConverter;

        private bool menuAdded;

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
        }

        /// <summary>
        /// Initialize all registred actions.
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
            if(imageConverter != null)
            {
                imageConverter.Dispose();
                imageConverter = null;
            }
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
                foreach (IPackageAction action in actions.Actions)
                    action.Initialize(this, menuManager, menuManager);
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
                tool.Window = ((Windows2)appObject.Windows).CreateToolWindow2(addInInstance, type.Assembly.Location, type.FullName, tool.Caption, guid, ref objToolWindow);

                if(objToolWindow == null)
                {
                    objToolWindow = LastCreatedPackageTool;
                    lastCreatedPackageTool = null;
                }

                // set proper icon retrieved from resources:
                if (tool.BitmapIndex > 0)
                {
                    Bitmap tabIcon = LoadImage(tool.BitmapIndex.ToString());

                    if (tabIcon != null)
                    {
                        // create object that will convert images if needed:
                        if(imageConverter == null)
                            imageConverter = new CustomImageConverter();

                        // repair the transparent-background color:
                        Bitmap tabNewIcon =
                            ReplaceColor(tabIcon, Color.FromArgb(255, 0, 254, 0), Color.FromArgb(255, 255, 0, 255));

                        // set the tab-picture image:
                        tool.Window.SetTabPicture(tabNewIcon.GetHbitmap());
                    }
                }

                if (!tool.IsFloating)
                {
                    tool.Window.Linkable = false;
                    tool.Window.IsFloating = false;
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
            }

            return objToolWindow;
        }

        private static Bitmap ReplaceColor(Image src, Color from, Color to)
        {
            if (src != null)
            {
                Bitmap result = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
                ColorMap map = new ColorMap();
                ImageAttributes attrs = new ImageAttributes();
                Graphics g = Graphics.FromImage(result);

                // set the color transformation:
                map.NewColor = to;
                map.OldColor = from;
                attrs.SetRemapTable(new ColorMap[] {map}, ColorAdjustType.Bitmap);

                g.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height), 0, 0, src.Width, src.Height,
                            GraphicsUnit.Pixel, attrs);
                g.Dispose();
                return result;
            }

            return null;
        }

        #endregion

        #region IPackageEnvironment Members

        /// <summary>
        /// Gets the VisualStudio IDE application object.
        /// </summary>
        public DTE2 DTE
        {
            get { return appObject; }
        }

        /// <summary>
        /// Gets the CodeEditPoint for current active document.
        /// </summary>
        public CodeEditPoint CurrentEditPoint
        {
            get
            {
                return IDEHelper.CreateEditPointForCurrentDocument(appObject);
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

            if (!toolWindows.TryGetValue(toolWindow, out t) || t == null)
            {
                toolWindow.Control = ((IToolCreator) this).CreateToolWindow(toolWindow) as Control;
                toolWindow.Control.Text = toolWindow.Caption;
                toolWindow.Window.Visible = true;

                // store for future usage:
                toolWindows.Add(toolWindow, toolWindow.Window);
                return true;
            }

            t.Visible = true;
            return false;

            //IDEHelper.ShowToolWindow(toolWindow.ToolWindowPaneType, assist, false, false);
        }

        /// <summary>
        /// Loads image from current package resources or satellite DLLs.
        /// </summary>
        /// <param name="name">Name of the image-resource to load.</param>
        public Bitmap LoadImage(string name)
        {
            try
            {
                // load proper satelite assembly:
                if (satelliteAssembly == null)
                {
                    satelliteAssembly = mainAssembly.GetSatelliteAssembly(
                            System.Threading.Thread.CurrentThread.CurrentUICulture);

                    satelliteResources = new ResourceManager("PublicResources", satelliteAssembly);
                }

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

        #endregion

        #region IServiceProvider Members

        ///<summary>
        ///Gets the service object of the specified type.
        ///</summary>
        ///
        ///<returns>
        ///A service object of type serviceType.-or- null if there is no service object of type serviceType.
        ///</returns>
        ///
        ///<param name="serviceType">An object that specifies the type of service object to get. </param><filterpriority>2</filterpriority>
        public object GetService(Type serviceType)
        {
            return IDEHelper.GetService(appObject as Microsoft.VisualStudio.OLE.Interop.IServiceProvider, serviceType);
        }

        #endregion
    }
}