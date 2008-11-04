using Pretorianie.Tytan.Core.Tracer;

namespace Pretorianie.Tytan.Core.Tracer
{
    /// <summary>
    /// Interface implemented by all kind log capture and filters.
    /// </summary>
    public interface ILogStorage
    {
        /// <summary>
        /// Late-initialize of given log class.
        /// </summary>
        void Start();

        /// <summary>
        /// Release all resources and stop capturing of log entries.
        /// </summary>
        void Stop();

        /// <summary>
        /// Store log message.
        /// </summary>
        void Write(LogImportance level, string text);
    }
}
