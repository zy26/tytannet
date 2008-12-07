using System;

namespace Pretorianie.Tytan.Core.Execution
{
    /// <summary>
    /// Delegate to common methods of objects implementing IQueuedItemAction interface.
    /// </summary>
    public delegate void QueuedTaskHandler(IQueuedTask i, EventArgs e);
}