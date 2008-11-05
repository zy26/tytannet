using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Pretorianie.Tytan.Core.TaskList
{
    /// <summary>
    /// Class that defines the custom task item.
    /// </summary>
    public class CustomTask : IVsTaskItem, IVsTaskItem3
    {
        protected IVsTaskProvider3 provider;
        protected string name;
        protected string toolTip;
        protected string comment;

        protected bool ignored;

        /// <summary>
        /// Init constructor of CustomTask.
        /// </summary>
        public CustomTask(IVsTaskProvider3 provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Init constructor of CustomTask.
        /// </summary>
        public CustomTask(IVsTaskProvider3 provider, string name, string toolTip, string comment, bool ignored)
        {
            this.provider = provider;
            this.name = name;
            this.toolTip = toolTip;
            this.comment = comment;
            this.ignored = ignored;
        }
        
        /// <summary>
        /// Gets the column description.
        /// This method should return 'true' if valid info has been set to fields, otherwise 'false' will send no data to VS IDE TaskList provider.
        /// </summary>
        protected virtual bool GetColumnValue(int fieldIndex, ref __VSTASKVALUETYPE type, ref __VSTASKVALUEFLAGS flags, ref object value, ref string accessibilityName)
        {
            return false;
        }

        #region IVsTaskItem Members

        int IVsTaskItem.CanDelete(out int pfCanDelete)
        {
            pfCanDelete = 0;
            return VSConstants.S_OK;
        }

        int IVsTaskItem.Category(VSTASKCATEGORY[] pCat)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.Column(out int piCol)
        {
            piCol = 0;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.Document(out string pbstrMkDocument)
        {
            pbstrMkDocument = null;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.HasHelp(out int pfHasHelp)
        {
            pfHasHelp = 1;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.ImageListIndex(out int pIndex)
        {
            pIndex = 0;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.IsReadOnly(VSTASKFIELD field, out int pfReadOnly)
        {
            pfReadOnly = 1;
            return VSConstants.S_OK;
        }

        int IVsTaskItem.Line(out int piLine)
        {
            piLine = 0;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.NavigateTo()
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.NavigateToHelp()
        {
            return VSConstants.S_OK;
        }

        int IVsTaskItem.OnDeleteTask()
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.OnFilterTask(int fVisible)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.SubcategoryIndex(out int pIndex)
        {
            pIndex = 0;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.get_Checked(out int pfChecked)
        {
            pfChecked = 0;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.get_Priority(VSTASKPRIORITY[] ptpPriority)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.get_Text(out string pbstrName)
        {
            pbstrName = null;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.put_Checked(int fChecked)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.put_Priority(VSTASKPRIORITY tpPriority)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem.put_Text(string bstrName)
        {
            return VSConstants.E_NOTIMPL;
        }

        #endregion

        #region IVsTaskItem3 Members

        int IVsTaskItem3.GetColumnValue(int iField, out uint ptvtType, out uint ptvfFlags, out object pvarValue, out string pbstrAccessibilityName)
        {
            __VSTASKVALUETYPE type = __VSTASKVALUETYPE.TVT_NULL;
            __VSTASKVALUEFLAGS flags = 0;

            pvarValue = null;
            pbstrAccessibilityName = string.Empty;

            // gets the column description:
            if (GetColumnValue(iField, ref type, ref flags, ref pvarValue, ref pbstrAccessibilityName))
            {
                ptvtType = (uint)type;
            }
            else
            {
                ptvtType = (uint)__VSTASKVALUETYPE.TVT_NULL;
                pvarValue = null;
                pbstrAccessibilityName = string.Empty;
            }

            // post-process flags:
            ptvfFlags = (uint)flags;
            if (Ignored)
            {
                ptvfFlags |= (uint)__VSTASKVALUEFLAGS.TVF_STRIKETHROUGH;
            }

            return VSConstants.S_OK;
        }

        int IVsTaskItem3.GetDefaultEditField(out int piField)
        {
            piField = -1;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem3.GetEnumCount(int iField, out int pnValues)
        {
            pnValues = 0;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem3.GetEnumValue(int iField, int iValue, out object pvarValue, out string pbstrAccessibilityName)
        {
            pvarValue = null;
            pbstrAccessibilityName = null;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem3.GetNavigationStatusText(out string pbstrText)
        {
            pbstrText = comment;
            return VSConstants.S_OK;
        }

        int IVsTaskItem3.GetSurrogateProviderGuid(out Guid pguidProvider)
        {
            pguidProvider = Guid.Empty;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem3.GetTaskName(out string pbstrName)
        {
            pbstrName = name;
            return VSConstants.S_OK;
        }

        int IVsTaskItem3.GetTaskProvider(out IVsTaskProvider3 ppProvider)
        {
            ppProvider = provider;
            return VSConstants.S_OK;
        }

        int IVsTaskItem3.GetTipText(int iField, out string pbstrTipText)
        {
            pbstrTipText = toolTip;
            return VSConstants.S_OK;
        }

        int IVsTaskItem3.IsDirty(out int pfDirty)
        {
            pfDirty = 0;
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem3.OnLinkClicked(int iField, int iLinkIndex)
        {
            return VSConstants.E_NOTIMPL;
        }

        int IVsTaskItem3.SetColumnValue(int iField, ref object pvarValue)
        {
            return VSConstants.E_NOTIMPL;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of Provider.
        /// </summary>
        public IVsTaskProvider3 Provider
        {
            get
            {
                return provider;
            }
            set
            {
                provider = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of Name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of ToolTip.
        /// </summary>
        public string ToolTip
        {
            get
            {
                return toolTip;
            }
            set
            {
                toolTip = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of Comment.
        /// </summary>
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of Ignored.
        /// </summary>
        public bool Ignored
        {
            get
            {
                return ignored;
            }
            set
            {
                ignored = value;
            }
        }

        #endregion
    }
}
