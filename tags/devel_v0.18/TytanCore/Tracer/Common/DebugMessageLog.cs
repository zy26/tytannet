using System.Diagnostics;

namespace Pretorianie.Tytan.Core.Tracer.Common
{
    /// <summary>
    /// Logger that redirects messages to standard Windows log output.
    /// </summary>
    public class DebugMessageLog : ILogStorage
    {
        #region ILogStorage Members

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Write(LogImportance level, string text)
        {
            Trace.WriteLine(text);
        }

        #endregion
    }
}
