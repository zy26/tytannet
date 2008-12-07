using System.Collections.Generic;
using System.Threading;

namespace Pretorianie.Tytan.Core.Execution
{
    /// <summary>
    /// Class that executes processing of specified elements inside separate thread.
    /// </summary>
    /// <typeparam name="T">Type of elements that should be processed.</typeparam>
    public class BackgroundProcessor<T> : IProcessor<T> where T : class
    {
        /// <summary>
        /// Delegate to method that processes stored elements. It will be called in separate thread,
        /// specially assigned to given instance of <c>BackgroundProcessor</c>.
        /// </summary>
        public delegate void ProcessHandler(IProcessor<T> sender, T item);

        /// <summary>
        /// Event generated each time new item is added to collection.
        /// </summary>
        public event ProcessHandler ProcessItem;

        /// <summary>
        /// Event generated when <c>BackgroundProcessor</c> is about to close and there is active item processed.
        /// </summary>
        public event ProcessHandler AbortItem;

        private readonly Queue<T> items;
        private Thread threadBckg;
        private readonly AutoResetEvent itemsActivator;

        private readonly object syncObject;
        private volatile bool isWorking;

        private T activeItem;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BackgroundProcessor()
            : this(32)
        {
        }

        /// <summary>
        /// Init constructor. Reserves the memory for given amount of elements.
        /// </summary>
        public BackgroundProcessor(int count)
        {
            items = new Queue<T>(count);
            itemsActivator = new AutoResetEvent(false);
            syncObject = new object();
        }

        /// <summary>
        /// Initialize instance.
        /// </summary>
        public void Initialize()
        {
            isWorking = true;
        }

        /// <summary>
        /// Add new item to processing.
        /// </summary>
        /// <param name="item">Item to process.</param>
        public void Add(T item)
        {
            lock (syncObject)
            {
                // add do collection:
                items.Enqueue(item);
                itemsActivator.Set();
            }

            if (threadBckg == null)
            {
                threadBckg = new Thread(ProcessItems);
                threadBckg.Start();
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
                itemsActivator.Set();

                if (AbortItem != null)
                {
                    lock (syncObject)
                    {
                        if (activeItem != null)
                            AbortItem(this, activeItem);
                    }
                }

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
                    if (ProcessItem != null)
                        ProcessItem(this, activeItem);

                    lock(syncObject)
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