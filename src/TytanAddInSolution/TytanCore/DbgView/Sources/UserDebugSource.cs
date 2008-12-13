namespace Pretorianie.Tytan.Core.DbgView.Sources
{
    /// <summary>
    /// Specialized source that broadcasts messages defined by the user.
    /// </summary>
    public class UserDebugSource : IDbgSource
    {
        private bool isWorking;
        private readonly string name;
        private readonly string description;

        /// <summary>
        /// Init constructor of UserDebugSource.
        /// </summary>
        public UserDebugSource(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        #region Implementation of IDbgSource

        public event DbgDataEventHandler DataReceived;

        /// <summary>
        /// Provides info if given source can be disabled at runtime by the user.
        /// </summary>
        public bool CanConfigureAtRuntime
        {
            get { return true; }
        }

        /// <summary>
        /// Reinitialize and start processing of messages.
        /// </summary>
        public void Start()
        {
            isWorking = true;
        }

        /// <summary>
        /// Stops receiving of the data.
        /// </summary>
        public void Close()
        {
            isWorking = false;
        }

        /// <summary>
        /// Gets the name of given source.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the module name of given source.
        /// </summary>
        public string Module
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the description of given source.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        #endregion

        /// <summary>
        /// Writes a specified debug message.
        /// </summary>
        public void Write(string message)
        {
            if (isWorking && DataReceived != null)
                DataReceived(this, 0, message);
        }

        /// <summary>
        /// Writes a specified debug message.
        /// </summary>
        public void Write(string format, params object[] args)
        {
            if (isWorking)
            {
                string m = string.Format(format, args);
                if (DataReceived != null)
                    DataReceived(this, 0, m);
            }
        }
    }
}
