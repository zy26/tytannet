using System;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Pretorianie.Tytan.Actions;
using Pretorianie.Tytan.Actions.Misc;
using Pretorianie.Tytan.Actions.Tools;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.Data;
using System.Reflection;

namespace Pretorianie.Tytan
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    [ComVisible(true)]
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private CustomAddInManager manager;
        private TytanCustomizator customizator;

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            if (manager == null)
            {
                customizator = new TytanCustomizator();
                manager = new CustomAddInManager((DTE2)application, (AddIn)addInInst, customizator,
                                                 Assembly.GetExecutingAssembly());
                //if (connectMode != ext_ConnectMode.ext_cm_UISetup)
                customizator.Initialize(manager);

                // register all known refactoring IPackageActions:
                manager.Add(new InitConstructorRefactor());
                manager.Add(new ExtractPropertyRefactor());
                //manager.Add(new ExpandEnumRefactor());
                // manager.Add(new ExtractResourceRefactor());
                manager.Add(new AssignReorderRefactor());
                manager.Add(new MultiRenameRefactor());
                manager.Add(new InsertionGuidRefactor());
                manager.Add(new InsertionClassNameAction());
                manager.Add(new InsertionPathRefactor());
                manager.Add(new InsertionDatabaseRefactor());
                if (manager.Version == ShellVersions.VS2005)
                    manager.Add(new OpenWindowsExplorer());
                manager.Add(new VisualStudioCloseQuestion());
                manager.Add(new ReferenceProjectAction());

                // manager.Add(new NativeImagePreviewPackageTool());
                manager.Add(new CommandViewPackageTool());
                manager.Add(new RegistryViewPackageTool());
                manager.Add(new EnvironmentVarsPackageTool());
                manager.Add(new DebugViewPackageTool());

                manager.Add(new AboutBoxAction());
            }

            // initialize VS IDE commands:
            manager.ApplicationInit(true /* connectMode == ext_ConnectMode.ext_cm_UISetup */);
            customizator.AfterApplicationInit(true);
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            manager.ApplicationExit(false /* true */);
            customizator.Destroy();
            manager = null;
            customizator = null;
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        #region IDTCommandTarget Members

        /// <summary>
        /// Execute command.
        /// </summary>
        public void Exec(string cmdName, vsCommandExecOption executeOption, ref object variantIn, ref object variantOut, ref bool handled)
        {
            manager.Execute(cmdName, ref handled);
        }

        /// <summary>
        /// Query status.
        /// </summary>
        public void QueryStatus(string cmdName, vsCommandStatusTextWanted neededText, ref vsCommandStatus statusOption, ref object commandText)
        {
            manager.QueryStatus(cmdName, neededText, ref statusOption, ref commandText);
        }

        #endregion
    }
}
