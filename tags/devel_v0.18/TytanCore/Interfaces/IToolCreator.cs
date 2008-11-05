namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Interface that allows the creation of the custom ToolWindows.
    /// </summary>
    public interface IToolCreator
    {
        /// <summary>
        /// Gets the current environment ProgID.
        /// </summary>
        string ProgID
        { get; }

        /// <summary>
        /// Creates a new tool window.
        /// </summary>
        object CreateToolWindow(IPackageToolWindow tool);
    }
}