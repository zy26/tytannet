namespace Pretorianie.Tytan.Core.DbgView
{
    /// <summary>
    /// Class that describes the process running in OS.
    /// </summary>
    public sealed class ProcessData
    {
        private readonly uint pid;
        private readonly string name;
        private readonly string mainModuleFileName;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public ProcessData(uint pid, string name, string mainModuleFileName)
        {
            this.pid = pid;
            this.mainModuleFileName = mainModuleFileName;
            this.name = name;
        }

        #region Properties

        /// <summary>
        /// Gets the PID.
        /// </summary>
        public uint PID
        {
            get { return pid; }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the file name of process' main module.
        /// </summary>
        public string MainModuleFileName
        {
            get { return mainModuleFileName; }
        }

        #endregion
    }
}
