using System.Drawing;
using System;

namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Class describing given package.
    /// </summary>
    public class PackageInfo
    {
        private readonly string friendlyName;
        private readonly string info;
        private readonly string description;
        private readonly Bitmap icon;
        private readonly Version version;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public PackageInfo(string friendlyName, string info, string description, Bitmap icon, Version version)
        {
            this.friendlyName = friendlyName;
            this.info = info;
            this.icon = icon;
            this.description = description.Replace(@"\r\n", Environment.NewLine);
            this.version = version;
        }

        #region Properties

        /// <summary>
        /// Gets the friendly name of the package.
        /// </summary>
        public string FriendlyName
        {
            get { return friendlyName; }
        }

        /// <summary>
        /// Gets the info text.
        /// </summary>
        public string Info
        {
            get { return info; }
        }

        /// <summary>
        /// Gets the description string of the package.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets the description icon.
        /// </summary>
        public Bitmap Icon
        {
            get { return icon; }
        }

        /// <summary>
        /// Gets the current version of an add-in.
        /// </summary>
        public Version Version
        {
            get { return version; }
        }

        #endregion
    }
}
