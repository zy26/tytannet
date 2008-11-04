using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Pretorianie.Tytan.Core.Events;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class that provides info about new versions.
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// Path to the text file with version info.
        /// </summary>
        private const string VersionURL = "http://tytannet.googlecode.com/files/CurrentVersion.txt";

        /// <summary>
        /// Path to the new version release page.
        /// </summary>
        private const string ReleaseURL = "http://www.codeplex.com/tytannet/Release/ProjectReleases.aspx";

        /// <summary>
        /// Name of the parameter that brings the version info.
        /// </summary>
        private const string ParamVersionName = "Version";


        private static readonly Version invalidVersion = new Version("0.0.0.0");
        private static Version currentVersion;
        private static Version serverVersion;
        private static WebClient versionClient;
        private static readonly List<VersionCheckHandler> listeners = new List<VersionCheckHandler>();
        private static readonly object syncObject = new object();
        private static readonly object syncList = new object();

        /// <summary>
        /// Checks for new version on the remote server and calls proper delegate as a confirmation.
        /// </summary>
        public static bool CheckVersion(VersionCheckHandler notification)
        {
            bool isVersionKnown;

            lock (syncObject)
            {
                if (versionClient == null)
                {
                    versionClient = new WebClient();
                    versionClient.DownloadStringCompleted += RemoteVersionCompleted;
                    versionClient.DownloadStringAsync(new Uri(VersionURL));
                }

                isVersionKnown = serverVersion != null;

                // remember to notify the client, as the result must be processed asynchronously:
                if (!isVersionKnown && notification != null)
                {
                    lock (syncList)
                    {
                        listeners.Add(notification);
                    }
                }
            }

            // perform the immediate notification call, as the version is already downloaded and known:
            if (isVersionKnown)
            {
                if (notification != null)
                    notification(currentVersion, serverVersion, ReleaseURL);
            }

            return isVersionKnown;
        }

        private static void RemoteVersionCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string[] data = e.Result != null
                                    ? e.Result.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                                    : null;
                string version = CurrentVersion.ToString();

                // parse received data for version info:
                if (data != null)
                {
                    foreach (string s in data)
                        if (s.StartsWith(ParamVersionName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            version = s.Substring(s.IndexOf(':') + 1).Trim();
                            break;
                        }
                }

                // remember the server version:
                lock (syncObject)
                {
                    try
                    {
                        serverVersion = new Version(version);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("Invalid version info retrieved from the server: '{0}'", version));
                        Trace.WriteLine(ex.Message);
                        serverVersion = InvalidVersion;
                    }
                }

                // both version should be known here, so fire the proper notifications:
                lock (syncList)
                {
                    foreach (VersionCheckHandler c in listeners)
                        c(currentVersion, serverVersion, ReleaseURL);

                    // and never call them again...
                    listeners.Clear();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Gets the current version of TytanNET.
        /// </summary>
        public static Version CurrentVersion
        {
            get
            {
                if (currentVersion == null)
                    currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                return currentVersion;
            }
        }

        /// <summary>
        /// Gets the indication which version number is invalid.
        /// </summary>
        public static Version InvalidVersion
        {
            get { return invalidVersion; }
        }
    }
}
