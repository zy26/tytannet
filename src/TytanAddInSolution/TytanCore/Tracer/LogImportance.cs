namespace Pretorianie.Tytan.Core.Tracer
{
    /// <summary>
    /// Level of importancy for the given log entry.
    /// </summary>
    public enum LogImportance
    {
        /// <summary>
        /// Warning entry.
        /// </summary>
        Warning,
        /// <summary>
        /// Error entry.
        /// </summary>
        Error,
        /// <summary>
        /// Normal message.
        /// </summary>
        Status
    }
}