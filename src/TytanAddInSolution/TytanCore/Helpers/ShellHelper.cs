using System;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Pretorianie.Tytan.Core.Data;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class that provides additional IDE support functionality for actions.
    /// </summary>
    public static class ShellHelper
    {
        private static IServiceProvider serviceProvider;
        private static IVsUIShell2 shell;

        /// <summary>
        /// Gets the instance of service provider for given DTE object.
        /// </summary>
        public static IServiceProvider GetServiceProvider(DTE2 appObject)
        {
            return serviceProvider ??
                   (serviceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider) appObject));
        }

        /// <summary>
        /// Gets the specified service from known service provider and for given GUID.
        /// </summary>
        public static object GetService(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider, Guid serviceGuid)
        {
            object objService = null;
            IntPtr objInPtr;
            Guid objSidGuid = serviceGuid;
            Guid objIidGuid = serviceGuid;

            int hResult = serviceProvider.QueryService(ref objSidGuid, ref objIidGuid, out objInPtr);
            if (hResult != 0)
                //Marshal.ThrowExceptionForHR(hResult);
                return null;

            if (objInPtr != IntPtr.Zero)
            {
                objService = Marshal.GetObjectForIUnknown(objInPtr);
                Marshal.Release(objInPtr);
            }

            return objService;
        }

        /// <summary>
        /// Gets the Visual Studio Shell main interface.
        /// </summary>
        public static IVsUIShell2 GetShellService(IServiceProvider provider)
        {
            if(serviceProvider == null)
                return null;

            return shell ?? (shell = provider.GetService(typeof(SVsUIShell)) as IVsUIShell2);
        }

        /// <summary>
        /// Gets the Visual Studio Shell main interface for givien DTE object.
        /// </summary>
        public static IVsUIShell2 GetShellService(DTE2 appObject)
        {
            return shell ?? GetShellService(GetServiceProvider(appObject));
        }

        /// <summary>
        /// Gets the specified service of given type from known service provider.
        /// </summary>
        public static object GetService(Microsoft.VisualStudio.OLE.Interop.IServiceProvider provider, Type serviceType)
        {
            return GetService(provider, serviceType.GUID);
        }

        /// <summary>
        /// Creates <see cref="CodeEditPoint"/> for current state of the IDE.
        /// </summary>
        public static CodeEditPoint CreateEditPointForCurrentDocument(DTE2 appObject)
        {
            if (appObject.ActiveDocument != null)
            {
                TextSelection selection = (TextSelection)(appObject.ActiveDocument.Selection);
                return new CodeEditPoint(appObject, selection, selection.TopPoint, appObject.ActiveDocument.ProjectItem);
            }
            
            return new CodeEditPoint(appObject, null, null, null);
        }

        /// <summary>
        /// Shows (or creates if needed) the specified tool window.
        /// </summary>
        public static void ShowToolWindow(Type toolWindowType, PackageAssist assist, bool transient, bool multiInstances)
        {
            // Get the instance number 0 of given tool window. Silent assumption is that this window
            // is single instance so this instance is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = assist.FindToolWindow(toolWindowType, 0, true, transient, multiInstances);
            if (window == null || window.Frame == null || !(window.Frame is IVsWindowFrame))
                throw new COMException("Can not create IVsWindowFrame object");

            ErrorHandler.ThrowOnFailure(((IVsWindowFrame) window.Frame).Show());
        }

        /// <summary>
        /// Gets the version of Visual Studio IDE.
        /// </summary>
        public static ShellVersions GetVersion(DTE2 appObject)
        {
            if (appObject != null)
            {
                if (appObject.Version == "8.0")
                    return ShellVersions.VS2005;
                if (appObject.Version == "9.0")
                    return ShellVersions.VS2008;

                //if (appObject.RegistryRoot.StartsWith(@"Software\Microsoft\VisualStudio\8.0",
                //                                      StringComparison.CurrentCultureIgnoreCase))
                //    return ShellVersions.VS2005;

                //if (appObject.RegistryRoot.StartsWith(@"Software\Microsoft\VisualStudio\9.0",
                //                                      StringComparison.CurrentCultureIgnoreCase))
                //    return ShellVersions.VS2008;
            }

            return ShellVersions.Unknown;
        }

        /// <summary>
        /// Gets the short string version depending on Visual Studio IDE version.
        /// </summary>
        public static string GetVersionShortString(DTE2 appObject)
        {
            switch (GetVersion(appObject))
            {
                case ShellVersions.VS2005:
                    return "8";
                case ShellVersions.VS2008:
                    return "9";

                default:
                    return "0";
            }
        }

        /// <summary>
        /// Gets the current Visual Studio IDE state.
        /// </summary>
        public static ShellModes GetMode(DTE2 appObject)
        {
            return GetMode(appObject.Mode);
        }

        /// <summary>
        /// Gets the current Visual Studio IDE state.
        /// </summary>
        public static ShellModes GetMode(vsIDEMode m)
        {
            switch (m)
            {
                case vsIDEMode.vsIDEModeDebug:
                    return ShellModes.Debug;

                default:
                    return ShellModes.Design;
            }
        }
    }
}
