using System;

namespace Pretorianie.Tytan.Core.Events
{
    /// <summary>
    /// Method handler for processing asynchronous version check.
    /// </summary>
    /// <param name="currentVersion">String containing the number of current version.</param>
    /// <param name="newVersion">String containing the number of the version stored on the server.</param>
    /// <param name="navigationURL">URL to the server where the new version can be downloaded.</param>
    public delegate void VersionCheckHandler(Version currentVersion, Version newVersion, string navigationURL);
}
