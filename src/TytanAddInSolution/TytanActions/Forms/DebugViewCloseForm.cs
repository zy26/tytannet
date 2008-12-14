using System;
using System.Collections.Generic;
using Pretorianie.Tytan.Core.DbgView.Sources;

namespace Pretorianie.Tytan.Forms
{
    public partial class DebugViewCloseForm : Core.BaseForms.BasePackageForm
    {
        /// <summary>
        /// Handler to method providing the list of currently active sources.
        /// </summary>
        public delegate IList<IDbgSource> RequestSourceHandler();
        /// <summary>
        /// Handler to method stopping the given source.
        /// </summary>
        public delegate void StopSourceHandler(IDbgSource s);

        private RequestSourceHandler refreshList;
        private StopSourceHandler stopSource;
        private IList<IDbgSource> list;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DebugViewCloseForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets given data to UI.
        /// </summary>
        public void InitializeUI(RequestSourceHandler refresh, StopSourceHandler stop)
        {
            refreshList = refresh;
            stopSource = stop;
            ActiveControl = sources;
            RefreshUI();
        }

        /// <summary>
        /// Refreshes info displayed on the screen.
        /// </summary>
        protected void RefreshUI()
        {
            sources.Items.Clear();

            if (refreshList != null)
            {
                list = refreshList();

                // get list of available data sources:
                if (list != null && list.Count > 0)
                {
                    foreach (IDbgSource s in list)
                        sources.Items.Add(s.Description);

                    sources.SelectedIndex = 0;
                }
            }
        }

        private void bttStop_Click(object sender, EventArgs e)
        {
            int i = sources.SelectedIndex;

            // if possible:
            if (list != null && i < list.Count && stopSource != null)
            {
                // stop source and remove from the list on the screen:
                stopSource(list[i]);
                RefreshUI();
            }
        }

        private void bttStopAll_Click(object sender, EventArgs e)
        {
            if (list != null && stopSource != null)
            {
                // stop all sources:
                foreach (IDbgSource s in list)
                    stopSource(s);

                // refresh the screen:
                RefreshUI();
            }
        }
    }
}
