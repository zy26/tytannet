using System;
using System.Diagnostics;

namespace Pretorianie.Tytan.Data
{
    /// <summary>
    /// Providers of comments displayed on 'Tips & Tricks' dialog.
    /// </summary>
    public class TipsProvider
    {
        private readonly int first;
        private readonly int last;
        private readonly Random rand;
        private int lastTip;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TipsProvider ()
        {
            lastTip = -1;
            rand = new Random();
            first = Convert.ToInt32(TipsTricks.Tips_First);
            last = Convert.ToInt32(TipsTricks.Tips_Last);
        }

        /// <summary>
        /// Gets the number of stored tips.
        /// </summary>
        public int Count
        {
            get { return last - first + 1; }
        }

        /// <summary>
        /// Gets the tips at given index.
        /// </summary>
        public string GetTip(int i, out bool isRtf)
        {
            string tip = string.Empty;

            isRtf = false;

            if (i < 0 || i > last)
                throw new ArgumentOutOfRangeException("i", "Invalid index of a tip to access");

            try
            {
                try
                {
                    tip = TipsTricks.ResourceManager.GetString("T" + i, TipsTricks.Culture);
                    lastTip = i;
                }
                catch
                {
                    isRtf = true;
                    tip = TipsTricks.ResourceManager.GetString("R" + i, TipsTricks.Culture);
                    lastTip = i;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }

            return tip;
        }

        /// <summary>
        /// Gets the random tip.
        /// </summary>
        public string GetRandomTip(out int i, out bool isRtf)
        {
            if (Count > 1)
            {
                // get the new tip, but different than the current one:
                do
                {
                    i = rand.Next(Count);
                } while (i == lastTip);
            }
            else
            {
                i = first;
            }

            return GetTip(i, out isRtf);
        }
    }
}
