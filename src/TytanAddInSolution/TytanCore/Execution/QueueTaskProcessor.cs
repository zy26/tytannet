using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Pretorianie.Tytan.Core.Execution
{
    /// <summary>
    /// Class that executes processing of specified elements inside one specialized thread.
    /// </summary>
    public class QueueTaskProcessor : IProcessor<IQueuedTask>
    {
        private readonly Queue<IQueuedTask> items;
        private Thread threadBckg;
        private readonly AutoResetEvent itemsActivator;

        private readonly object syncObject;
        private volatile bool isWorking;

        private IQueuedTask activeItem;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public QueueTaskProcessor()
            : this(32)
        {
        }

        /// <summary>
        /// Init constructor. Reservers memory for given number of elements to be places inside the queue.
        /// </summary>
        public QueueTaskProcessor(int count)
        {
            items = new Queue<IQueuedTask>(count);
            itemsActivator = new AutoResetEvent(false);
            syncObject = new object();
        }

        /// <summary>
        /// Initialize instance.
        /// </summary>
        public void Initialize()
        {
            if (threadBckg == null)
            {
                isWorking = true;
                threadBckg = new Thread(ProcessItems);
                threadBckg.Start();
            }
        }

        /// <summary>
        /// Add new item to processing.
        /// </summary>
        /// <param name="item">Item to process.</param>
        public void Add(IQueuedTask item)
        {
            lock (syncObject)
            {
                // add do collection:
                items.Enqueue(item);
                itemsActivator.Set();
            }
        }

        /// <summary>
        /// Stop background thread processing.
        /// </summary>
        public void Close()
        {
            isWorking = false;

            // stop thread:
            if (threadBckg != null)
            {
                // abort current action:
                lock (syncObject)
                {
                    if (activeItem != null)
                        activeItem.Abort();
                }

                itemsActivator.Set();
                threadBckg.Join();
                threadBckg = null;
            }
        }

        /// <summary>
        /// Gets the current number of stored tasks.
        /// </summary>
        public int Count
        {
            get
            {
                lock (syncObject)
                {
                    return items.Count;
                }
            }
        }

        /// <summary>
        /// Remove all items from processing queue.
        /// </summary>
        public void Clear()
        {
            lock (syncObject)
            {
                items.Clear();
            }
        }

        private void ProcessItems()
        {
            while (isWorking)
            {
                if (items.Count > 0)
                {
                    // get item from collection:
                    lock (syncObject)
                    {
                        activeItem = items.Dequeue();
                    }

                    // execute:
                    try
                    {
                        activeItem.Execute(this);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                        Trace.WriteLine(ex.StackTrace);
                    }

                    lock (syncObject)
                    {
                        activeItem = null;
                    }
                }
                else
                    // wait for new items:
                    itemsActivator.WaitOne();
            }
        }
    }
}