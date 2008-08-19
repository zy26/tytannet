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
    /// Helper class that provides additional IDE support functionality for AddIns.
    /// </summary>
    public static class IDEHelper
    {
        /// <summary>
        /// Gets the specified service from known service provider and for given GUID.
        /// </summary>
        public static object GetService(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider, Guid serviceGUID)
        {
            object objService = null;
            IntPtr objInPtr;
            int hResult;
            Guid objSIDGuid = serviceGUID;
            Guid objIIDGuid = serviceGUID;

            hResult = serviceProvider.QueryService(ref objSIDGuid, ref objIIDGuid, out objInPtr);
            if (hResult != 0)
                //Marshal.ThrowExceptionForHR(hResult);
                return null;
            else
                if (objInPtr != IntPtr.Zero)
                {
                    objService = Marshal.GetObjectForIUnknown(objInPtr);
                    Marshal.Release(objInPtr);
                }

            return objService;
        }

        /// <summary>
        /// Gets the specified service of given type from known service provider.
        /// </summary>
        public static object GetService(Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider, Type serviceType)
        {
            return GetService(serviceProvider, serviceType.GUID);
        }

        /// <summary>
        /// Creates EditPoint for current state of the IDE.
        /// </summary>
        public static CodeEditPoint CreateEditPointForCurrentDocument(DTE2 appObject)
        {
            if (appObject.ActiveDocument != null)
            {
                TextSelection selection = (TextSelection)(appObject.ActiveDocument.Selection);
                return new CodeEditPoint(appObject, selection, selection.TopPoint, appObject.ActiveDocument.ProjectItem);
            }
            else
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
    }
}
