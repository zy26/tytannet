using System;
using Pretorianie.Tytan.Actions.Misc;
using Pretorianie.Tytan.Core.Execution;

namespace Pretorianie.Tytan.Actions.Internals
{
    /// <summary>
    /// Base class for actions performed over the projects.
    /// </summary>
    internal abstract class BaseReferenceTask : IQueuedTask
    {
        protected readonly ReferenceProjectAction action;
        private bool isAborted;

        /// <summary>
        /// Init constructor of RemoveAllProjectsTask.
        /// </summary>
        protected BaseReferenceTask(ReferenceProjectAction action)
        {
            this.action = action;
        }

        #region Implementation of IQueuedTask

        public event QueuedTaskHandler Completed;

        /// <summary>
        /// Process item actions run inside separate thread.
        /// </summary>
        public void Execute(IProcessor<IQueuedTask> processor)
        {
            // check if there are other tasks in the queue
            // - that is - more than one action to be executed
            // - and the result of the current one
            // will be overriden:
            if (processor.Count == 0 && !isAborted)
            {
                System.Threading.Thread.Sleep(2000);

                // check one again after some time:
                if (processor.Count == 0 && !isAborted)
                    Execute();
            }
            // notify:
            if (Completed != null)
                Completed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Execute the proper action.
        /// </summary>
        protected abstract void Execute();

        /// <summary>
        /// Stop execution of current action.
        /// </summary>
        public void Abort()
        {
            isAborted = true;
        }

        #endregion
    }
}
