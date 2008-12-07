namespace Pretorianie.Tytan.Core.DbgView.Sources
{
    /// <summary>
    /// Interface implemented by debug message-data source object.
    /// </summary>
    public interface IDbgSource
    {
        /// <summary>
        /// Event fired each time new pack of raw data has been received.
        /// No synchronization and multi-line support guaranteed.
        /// </summary>
        event DbgDataEventHandler DataReceived;

        /// <summary>
        /// Provides info if given source can be disabled at runtime by the user.
        /// </summary>
        bool CanConfigureAtRuntime
        { get; }

        /// <summary>
        /// Reinitialize and start processing of messages.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops receiving of the data.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets the name of given source.
        /// </summary>
        string Name
        { get; }

        /// <summary>
        /// Gets the module name of given source.
        /// </summary>
        string Module
        { get; }

        /// <summary>
        /// Gets the description of given source.
        /// </summary>
        string Description
        { get; }
    }
}
