namespace Pretorianie.Tytan.Core.Execution
{
    /// <summary>
    /// Interface implemented by tasks.
    /// </summary>
    public interface IQueuedTask
    {
        /// <summary>
        /// Event fired with Execution finished with success.
        /// </summary>
        event QueuedTaskHandler Completed;

        /// <summary>
        /// Process item actions run inside separate thread.
        /// </summary>
        void Execute(IProcessor<IQueuedTask> processor);

        /// <summary>
        /// Stop execution of current action.
        /// </summary>
        void Abort();
    }
}