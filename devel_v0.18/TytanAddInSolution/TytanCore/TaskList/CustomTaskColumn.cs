using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Pretorianie.Tytan.Core.TaskList
{
    /// <summary>
    /// Type that describes the special column behaviour.
    /// </summary>
    [Flags]
    public enum CustomTaskColumnFlags
    {
        /// <summary>
        /// Flag that indicates whether the user can change the column's visibility.
        /// </summary>
        AllowHide = 0x0001,
        /// <summary>
        /// Flag that indicates whether the user can sort by clicking the column's header.
        /// </summary>
        AllowUserSort = 0x0002,
        /// <summary>
        /// Flag that indicates that the column is sorted in descending order. Ascending order is the default.
        /// </summary>
        DescendingSort = 0x0004,
        /// <summary>
        /// Flag that indicates whether a sort arrow is shown in the column header when the list is sorted by this column.
        /// </summary>
        ShowSortArrow = 0x0008,
        /// <summary>
        /// Flag that indicates whether the task list can automatically resize the column to make content fit better.
        /// </summary>
        DynamicSize = 0x0010,
        /// <summary>
        /// Flag that indicated that the column width will capture the whole available space.
        /// </summary>
        FitContent = 0x0020,
        /// <summary>
        /// Flag that indicates whether the column may be dragged to another position by the user.
        /// </summary>
        Moveable = 0x0040,
        /// <summary>
        /// Flag that indicates whether the column may be resized by the user.
        /// </summary>
        Sizeable = 0x0080,
        /// <summary>
        /// Default set of flags.
        /// </summary>
        Default = AllowHide | AllowUserSort | Moveable,
        /// <summary>
        /// Default set of flags + Sizeable.
        /// </summary>
        DefaultSizeable = Default | Sizeable,
        /// <summary>
        /// Default set of flags + Sizeable + FitContent.
        /// </summary>
        DefaultFitContent = Default | Sizeable | FitContent | DynamicSize
    }

    /// <summary>
    /// Class that describes column inside the task list.
    /// </summary>
    public class CustomTaskColumnInfo
    {
        private string header;
        private string name;
        private string toolTip;

        private int widthDefault;
        private int widthMin;

        private int sortPriority = -1;
        private int fieldNumber;
        private int imageIndex;

        private bool visible = true;
        private CustomTaskColumnFlags flags = CustomTaskColumnFlags.Default;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CustomTaskColumnInfo()
        {
        }

        /// <summary>
        /// Init constructor of CustomTaskColumnInfo.
        /// </summary>
        public CustomTaskColumnInfo(string name, string toolTip, int widthDefault, int widthMin, int sortPriority, int fieldNumber, int imageIndex, bool visible, CustomTaskColumnFlags flags)
        {
            this.header = null;
            this.name = name;
            this.toolTip = (string.IsNullOrEmpty(toolTip) ? name : toolTip);
            this.widthDefault = widthDefault;
            this.widthMin = widthMin;
            this.sortPriority = sortPriority;
            this.fieldNumber = fieldNumber;
            this.imageIndex = imageIndex;
            this.visible = visible;
            this.flags = flags;
        }

        /// <summary>
        /// Init constructor of CustomTaskColumnInfo.
        /// </summary>
        public CustomTaskColumnInfo(string header, string name, string toolTip, int widthDefault, int widthMin, int sortPriority, int fieldNumber, bool visible, CustomTaskColumnFlags flags)
        {
            this.header = header;
            this.name = name;
            this.toolTip = (string.IsNullOrEmpty(toolTip) ? header : toolTip);
            this.widthDefault = widthDefault;
            this.widthMin = widthMin;
            this.sortPriority = sortPriority;
            this.fieldNumber = fieldNumber;
            this.imageIndex = -1;
            this.visible = visible;
            this.flags = flags;
        }

        /// <summary>
        /// Converts ColumnInfo structure to class used internally by VisualStudio IDE.
        /// </summary>
        internal VSTASKCOLUMN ToVsTaskColumn()
        {
            VSTASKCOLUMN x;

            x.bstrHeading = header;
            x.bstrCanonicalName = name;
            x.bstrLocalizedName = name;
            x.bstrTip = toolTip;
            x.cxDefaultWidth = widthDefault;
            x.cxMinWidth = widthMin;
            x.iDefaultSortPriority = sortPriority;
            x.iField = fieldNumber;
            x.iImage = imageIndex;
            x.fVisibleByDefault = visible ? 1 : 0;

            // parse flags:
            x.fAllowHide = (flags & CustomTaskColumnFlags.AllowHide) == CustomTaskColumnFlags.AllowHide ? 1 : 0;
            x.fAllowUserSort = (flags & CustomTaskColumnFlags.AllowUserSort) == CustomTaskColumnFlags.AllowUserSort ? 1 : 0;
            x.fDescendingSort = (flags & CustomTaskColumnFlags.DescendingSort) == CustomTaskColumnFlags.DescendingSort ? 1 : 0;
            x.fDynamicSize = (flags & CustomTaskColumnFlags.DynamicSize) == CustomTaskColumnFlags.DynamicSize ? 1 : 0;
            x.fFitContent = (flags & CustomTaskColumnFlags.FitContent) == CustomTaskColumnFlags.FitContent ? 1 : 0;
            x.fMoveable = (flags & CustomTaskColumnFlags.Moveable) == CustomTaskColumnFlags.Moveable ? 1 : 0;
            x.fShowSortArrow = (flags & CustomTaskColumnFlags.ShowSortArrow) == CustomTaskColumnFlags.ShowSortArrow ? 1 : 0;
            x.fSizeable = (flags & CustomTaskColumnFlags.Sizeable) == CustomTaskColumnFlags.Sizeable ? 1 : 0;

            return x;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the value of Header.
        /// </summary>
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
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
        /// Gets or sets the value of WidthDefault.
        /// </summary>
        public int WidthDefault
        {
            get
            {
                return widthDefault;
            }
            set
            {
                widthDefault = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of WidthMin.
        /// </summary>
        public int WidthMin
        {
            get
            {
                return widthMin;
            }
            set
            {
                widthMin = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of SortPriority.
        /// </summary>
        public int SortPriority
        {
            get
            {
                return sortPriority;
            }
            set
            {
                sortPriority = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of FieldNumber.
        /// </summary>
        public int FieldNumber
        {
            get
            {
                return fieldNumber;
            }
            set
            {
                fieldNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of ImageIndex.
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return imageIndex;
            }
            set
            {
                imageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of Visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of Flags.
        /// </summary>
        public CustomTaskColumnFlags Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        #endregion
    }

}
