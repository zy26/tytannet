namespace Pretorianie.Tytan.Core.Data
{
    /// <summary>
    /// Enumeration describing the current state of Visual Studio IDE.
    /// </summary>
    public enum ShellModes
    {
        /// <summary>
        /// IDE is in design mode. Developer is editing source-code and resource files.
        /// </summary>
        Design,
        /// <summary>
        /// IDE is currently debugging solution.
        /// </summary>
        Debug,
        /// <summary>
        /// IDE runs the solution executable.
        /// </summary>
        ApplicationRun
    }
}
