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
        private const string VersionUrlGoogle = "http://tytannet.googlecode.com/files/CurrentVersion.txt";
        /// <summary>
        /// Path to the wiki page with version info.
        /// </summary>
        private const string VersionUrlCodeplex = "http://www.codeplex.com/tytannet/Wiki/View.aspx?title=CurrentConfigurationInfo";

        /// <summary>
        /// Path to the new version release page.
        /// </summary>
        private const string ReleaseURL = "http://www.codeplex.com/tytannet/Release/ProjectReleases.aspx";

        /// <summary>
        /// List of characters that can be separators between parameter name and its value.
        /// </summary>
        private static readonly char[] ParamValueSeparators = new char[] {':', '='};
        /// <summary>
        /// Name of the parameter that brings the version info.
        /// </summary>
        private const string ParamVersionName = "Version";


        /// <summary>
        /// String that appears at the beginning of version-info section.
        /// </summary>
        private const string StartVersionSection = "### Config Start ###";
        /// <summary>
        /// String that appears at the end of the version info section.
        /// </summary>
        private const string EndVersionSection = "### Config End ###";


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
                    versionClient.DownloadStringCompleted += RemoteCodeplexVersionCompleted;
                    versionClient.DownloadStringAsync(new Uri(VersionUrlCodeplex));
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

        /// <summary>
        /// Operate on a raw-file received from code.google.com site.
        /// </summary>
        private static void RemoteGoogleVersionCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            ParseVersionString(e.Result);
        }

        /// <summary>
        /// Operate on a wiki-page content received from www.codeplex.com site.
        /// </summary>
        private static void RemoteCodeplexVersionCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string data = e.Result;

            if (!string.IsNullOrEmpty(e.Result))
            {
                int start = data.IndexOf(StartVersionSection);
                int end = data.IndexOf(EndVersionSection);

                if (start < 0 || end < 0)
                    data = null;
                else
                {
                    // cut the whole text between [Start] & [End]:
                    data = data.Substring(start + StartVersionSection.Length, end - start - StartVersionSection.Length);

                    // and finally replace all HTML new line characters:
                    data =
                        data.Replace("<BR />", Environment.NewLine).Replace("<br />", Environment.NewLine).Replace(
                            "<BR/>", Environment.NewLine).Replace("<br/>", Environment.NewLine);
                }
            }

            ParseVersionString(data);
        }


        private static void ParseVersionString(string versionString)
        {
            try
            {
                string[] data = string.IsNullOrEmpty(versionString)
                                    ? null
                                    : versionString.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                string version = CurrentVersion.ToString();

                // parse received data for version info:
                if (data != null)
                {
                    foreach (string s in data)
                    {
                        if (s.StartsWith(ParamVersionName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            version = s.Substring(s.IndexOfAny(ParamValueSeparators) + 1).Trim();
                            break;
                        }
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
