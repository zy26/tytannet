namespace Pretorianie.Tytan.Core.Execution
{
    /// <summary>
    /// Interface implemented by background processor managers.
    /// </summary>
    public interface IProcessor<T> where T : class
    {
        /// <summary>
        /// Gets the current number of stored tasks.
        /// </summary>
        int Count
        { get; }

        /// <summary>
        /// Removes all stored tasks.
        /// </summary>
        void Clear();

        /// <summary>
        /// Adds new task to execute.
        /// </summary>
        void Add(T task);
    }
}
