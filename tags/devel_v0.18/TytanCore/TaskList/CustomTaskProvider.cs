using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Pretorianie.Tytan.Core.TaskList
{
    /// <summary>
    /// Fully customizable TaskList provider.
    /// </summary>
    public class CustomTaskProvider : IVsTaskProvider, IVsTaskProvider3
    {
        /// <summary>
        /// Handler that receives the CustomTask items from external source.
        /// </summary>
        public delegate void RequestItemsHandler(CustomTaskProvider sender, object filter, out IList<CustomTask> items);

        private Guid guid;
        private string name;
        private readonly IServiceProvider serviceProvider;
        private uint cookie;

        private Guid toolbarGuid;
        private uint toolbarCmdID;

        private ImageList imageList;
        private IList<CustomTaskColumnInfo> columns;

        private object filter;
        private bool showIgnoredTasks = true;

        /// <summary>
        /// Event fired when task list requires refresh and external sources should provide task items.
        /// </summary>
        public event RequestItemsHandler RequestItems;

        #region P/Invoke

        [DllImport("comctl32.dll")]
        private static extern IntPtr ImageList_Duplicate(IntPtr originalHandle);

        #endregion

        /// <summary>
        /// Init constructor.
        /// Stores the provider GUID and name.
        /// </summary>
        public CustomTaskProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Registers current task provider inside the VisualStudio IDE.
        /// </summary>
        public void Register(string guid, string name)
        {
            // get access to TaskList:
            IVsTaskList taskListService = serviceProvider.GetService(typeof(SVsTaskList)) as IVsTaskList;
            IVsTaskList2 taskListService2 = taskListService as IVsTaskList2;

            this.guid = new Guid(guid);
            this.name = name;

            if (taskListService != null && taskListService2 != null)
            {
                ErrorHandler.ThrowOnFailure(taskListService.RegisterTaskProvider(this, out cookie));
                ErrorHandler.ThrowOnFailure(taskListService2.SetActiveProvider(ref this.guid));
            }
        }

        /// <summary>
        /// Registers toolbar for given TaskList provider.
        /// </summary>
        public void RegisterToolbar(Guid guid, uint cmdID)
        {
            toolbarGuid = guid;
            toolbarCmdID = cmdID;
        }

        /// <summary>
        /// Registers custom ImageList to be used during displaying columns and items.
        /// </summary>
        public void RegisterImageList(ImageList imageList)
        {
            this.imageList = imageList;
        }

        /// <summary>
        /// Registers new custom column for the task list.
        /// </summary>
        public void RegisterColumn(string name, string toolTip, int widthDefault, int widthMin, int sortPriority, int fieldNumber, int imageIndex, bool visible, CustomTaskColumnFlags flags)
        {
            RegisterColumn(new CustomTaskColumnInfo(name, toolTip, widthDefault, widthMin, sortPriority, fieldNumber, imageIndex, visible, flags));
        }

        /// <summary>
        /// Registers new custom column for the task list.
        /// </summary>
        public void RegisterColumn(string header, string name, string toolTip, int widthDefault, int widthMin, int sortPriority, int fieldNumber, bool visible, CustomTaskColumnFlags flags)
        {
            RegisterColumn(new CustomTaskColumnInfo(header, name, toolTip, widthDefault, widthMin, sortPriority, fieldNumber, visible, flags));
        }

        /// <summary>
        /// Registers new custom column for the task list.
        /// </summary>
        public void RegisterColumn(CustomTaskColumnInfo column)
        {
            if (columns == null)
                columns = new List<CustomTaskColumnInfo>();

            columns.Add(column);
        }

        #region Properties

        /// <summary>
        /// Checks if is currently selected provider.
        /// </summary>
        public bool IsActiveProvider
        {
            get
            {
                IVsTaskList2 taskList = serviceProvider.GetService(typeof (SVsTaskList)) as IVsTaskList2;
                IVsTaskProvider activeProvider;

                if (taskList != null)
                    return taskList.GetActiveProvider(out activeProvider) == VSConstants.S_OK && activeProvider == this;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets or sets the indication if ignored tasks should be added to the final list.
        /// </summary>
        public bool ShowIgnoredTasks
        {
            get { return showIgnoredTasks; }
            set { showIgnoredTasks = value; }
        }

        /// <summary>
        /// Gets or sets the filter used during CustomTask enumeration.
        /// </summary>
        public object Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh the TaskList on the screen.
        /// </summary>
        public void Refresh()
        {
            IVsTaskList taskList = serviceProvider.GetService(typeof (SVsTaskList)) as IVsTaskList;

            if (taskList != null)
                taskList.RefreshTasks(cookie);
        }

        #endregion

        #region IVsTaskProvider Members

        int IVsTaskProvider.EnumTaskItems(out IVsEnumTaskItems ppenum)
        {
            IList<CustomTask> list = null;

            // ask external source for items:
            if (RequestItems != null)
                RequestItems(this, filter, out list);

            if (list != null)
                ppenum = new CustomTaskEnumerator(list, showIgnoredTasks);
            else
                ppenum = null;

            return VSConstants.S_OK;
        }

        int IVsTaskProvider.ImageList(out IntPtr phImageList)
        {
            if (imageList == null)
                phImageList = IntPtr.Zero;
            else
                phImageList = ImageList_Duplicate(imageList.Handle);

            return VSConstants.S_OK;
        }

        int IVsTaskProvider.OnTaskListFinalRelease(IVsTaskList pTaskList)
        {
            if (pTaskList != null && cookie != 0)
            {
                pTaskList.UnregisterTaskProvider(cookie);
                cookie = 0;
                return VSConstants.S_OK;
            }

            return VSConstants.S_FALSE;
        }

        int IVsTaskProvider.ReRegistrationKey(out string pbstrKey)
        {
            pbstrKey = null;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskProvider.SubcategoryList(uint cbstr, string[] rgbstr, out uint pcActual)
        {
            pcActual = 0;
            return VSConstants.E_NOTIMPL;
        }

        #endregion

        #region IVsTaskProvider3 Members

        int IVsTaskProvider3.GetColumn(int iColumn, VSTASKCOLUMN[] pColumn)
        {
            if (columns != null && iColumn >= 0 && iColumn < columns.Count)
            {
                pColumn[0] = columns[iColumn].ToVsTaskColumn();
                return VSConstants.S_OK;
            }
            else
            {
                return VSConstants.E_INVALIDARG;
            }
        }

        int IVsTaskProvider3.GetColumnCount(out int pnColumns)
        {
            pnColumns = (columns != null ? columns.Count : 0);
            return VSConstants.S_OK;
        }

        int IVsTaskProvider3.GetProviderFlags(out uint tpfFlags)
        {
            tpfFlags = (uint)(__VSTASKPROVIDERFLAGS.TPF_ALWAYSVISIBLE | __VSTASKPROVIDERFLAGS.TPF_NOAUTOROUTING);
            return VSConstants.S_OK;
        }

        int IVsTaskProvider3.GetProviderGuid(out Guid pguidProvider)
        {
            pguidProvider = guid;
            return VSConstants.S_OK;
        }

        int IVsTaskProvider3.GetProviderName(out string pbstrName)
        {
            pbstrName = name;
            return VSConstants.S_OK;
        }

        int IVsTaskProvider3.GetProviderToolbar(out Guid pguidGroup, out uint pdwID)
        {
            pguidGroup = toolbarGuid;
            pdwID = toolbarCmdID;

            return VSConstants.S_FALSE;
        }

        int IVsTaskProvider3.GetSurrogateProviderGuid(out Guid pguidProvider)
        {
            pguidProvider = Guid.Empty;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskProvider3.OnBeginTaskEdit(IVsTaskItem pItem)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskProvider3.OnEndTaskEdit(IVsTaskItem pItem, int fCommitChanges, out int pfAllowChanges)
        {
            pfAllowChanges = 0;
            return VSConstants.E_NOTIMPL;
        }

        #endregion
    }
}
