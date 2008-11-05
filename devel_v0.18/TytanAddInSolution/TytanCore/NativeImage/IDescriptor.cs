namespace Pretorianie.Tytan.Core.NativeImage
{
    /// <summary>
    /// This is the basic interface implemented by all NativeImage's objects.
    /// </summary>
    public interface IDescriptor
    {
        /// <summary>
        /// Gets the size of given element in bytes.
        /// </summary>
        uint Size
        { get; }

        /// <summary>
        /// Gets the name of current element.
        /// This value may not be unique.
        /// </summary>
        string Name
        { get; }

        /// <summary>
        /// Gets the description of current element.
        /// </summary>
        string Description
        { get; }

        /// <summary>
        /// Gets or sets other associated object at run-time.
        /// </summary>
        object Tag
        { get; set; }
    }
}
