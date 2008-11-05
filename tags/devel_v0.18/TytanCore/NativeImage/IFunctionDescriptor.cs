namespace Pretorianie.Tytan.Core.NativeImage
{
    public interface IFunctionDescriptor : IDescriptor
    {
        /// <summary>
        /// Gets the name of NativeImage source file or other object.
        /// </summary>
        string SourceName
        { get; }

        string MangledName
        { get; }
    }
}
