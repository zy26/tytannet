using System;
using System.Diagnostics;
using System.ComponentModel;

namespace Pretorianie.Tytan.Core.DbgView
{
    /// <summary>
    /// Class that describes the debug message.
    /// It is also used later to show in PropertyWindow of VisualStudio IDE as a read-only class.
    /// </summary>
    public class DebugViewData
    {
        /// <summary>
        /// Gets the chars that split different parts of the message.
        /// </summary>
        public static readonly char[] SplitChars = new char[] { '|' };

        private readonly uint pid;
        private readonly string message;
        private readonly string processName;
        private readonly string processPath;
        private DateTime creation;

        /// <summary>
        /// Init constructor of DebugViewData.
        /// </summary>
        public DebugViewData(uint pid, string message)
        {
            this.pid = pid;
            this.message = message;
            
            processName = Process.GetProcessById((int)pid).ProcessName;
            processPath = Process.GetProcessById((int)pid).MainModule.FileName;
            creation = DateTime.Now;
        }

        /// <summary>
        /// Internal init constructor.
        /// Only for use when data imported from file.
        /// </summary>
        internal DebugViewData(uint pid, string processName, string processPath, DateTime creation, string message)
        {
            this.pid = pid;
            this.message = message;
            this.processName = processName;
            this.processPath = processPath;
            this.creation = creation;
        }

        #region Properties

        /// <summary>
        /// PID of the process that created the message.
        /// </summary>
        [Description("ID of process that created the message.")]
        [Category("Creator")]
        public uint PID
        {
            get { return pid; }
        }

        /// <summary>
        /// Content of the message.
        /// </summary>
        [Description("Content of the message.")]
        [Category("Data")]
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Message divided into understandable parts.
        /// </summary>
        [Description("Message divided into understandable parts.")]
        [Category("Data")]
        public string[] SplitMessage
        {
            get { return (string.IsNullOrEmpty(message) ? null : message.Split(SplitChars)); }
        }

        /// <summary>
        /// Name of the process that created the message.
        /// </summary>
        [Description("Name of the process that created the message.")]
        [Category("Creator")]
        public string ProcessName
        {
            get { return processName; }
        }

        /// <summary>
        /// Name of the process file.
        /// </summary>
        [Description("Name of the process DLL or EXE file.")]
        [Category("Creator")]
        public string ProcessPath
        {
            get { return processPath; }
        }
        
        /// <summary>
        /// Creation time of the received message.
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get { return creation; }
        }

        /// <summary>
        /// Creation time of teh received message.
        /// </summary>
        [Browsable(false)]
        public string CreationTime
        {
            get { return creation.ToString("HH:mm:ss.") + creation.Millisecond; }
        }

        /// <summary>
        /// Creation time of teh received message.
        /// </summary>
        [Description("Creation time of the received message.")]
        [Category("Creator")]
        public string CreationDateTime
        {
            get { return string.Format("{0} {1}", creation.ToShortDateString(), CreationTime); }
        }

        #endregion
    }
}
