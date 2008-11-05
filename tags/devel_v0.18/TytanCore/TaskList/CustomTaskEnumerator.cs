using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Pretorianie.Tytan.Core.TaskList
{
    /// <summary>
    /// Class that enumerates CustomTask collection.
    /// </summary>
    class CustomTaskEnumerator : IVsEnumTaskItems
    {
        private readonly IList<CustomTask> items;
        private readonly bool showIgnored;
        private int nextIndex = 0;

        /// <summary>
        /// Init constructor.
        /// It duplicates the given collection and then provides enumeration interface.
        /// </summary>
        public CustomTaskEnumerator(IList<CustomTask> items, bool showIgnored)
        {
            this.showIgnored = showIgnored;
            this.items = new List<CustomTask>(items);
        }

        #region IVsEnumTaskItems Members

        int IVsEnumTaskItems.Clone(out IVsEnumTaskItems ppenum)
        {
            ppenum = new CustomTaskEnumerator(items, showIgnored);
            return VSConstants.S_OK;
        }

        int IVsEnumTaskItems.Next(uint celt, IVsTaskItem[] rgelt, uint[] pceltFetched)
        {
            pceltFetched[0] = 0;

            while (pceltFetched[0] < celt && nextIndex < items.Count)
            {
                if (showIgnored || !items[nextIndex].Ignored)
                {
                    rgelt[pceltFetched[0]] = items[nextIndex];
                    pceltFetched[0]++;
                }
                ++nextIndex;
            }

            if (pceltFetched[0] == celt)
            {
                return VSConstants.S_OK;
            }
            else
            {
                return VSConstants.S_FALSE;
            }
        }

        int IVsEnumTaskItems.Reset()
        {
            nextIndex = 0;
            return VSConstants.S_OK;
        }

        int IVsEnumTaskItems.Skip(uint celt)
        {
            IVsTaskItem[] xItems = new IVsTaskItem[celt];
            uint[] fetched = new uint[] { 0 };

            return (this as IVsEnumTaskItems).Next(celt, xItems, fetched);
        }

        #endregion
    }
}
