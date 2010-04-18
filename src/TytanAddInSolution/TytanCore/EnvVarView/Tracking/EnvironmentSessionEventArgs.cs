using System;

namespace Pretorianie.Tytan.Core.EnvVarView.Tracking
{
    /// <summary>
    /// Arguments passed with 
    /// </summary>
    public class EnvironmentSessionEventArgs :EventArgs
    {
        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentSessionEventArgs(int count)
        {
            Count = count;
        }

        #region Properties

        /// <summary>
        /// Gets the number of changed variables within the current session.
        /// </summary>
        public int Count
        {
            get; protected set;
        }

        #endregion
    }
}
