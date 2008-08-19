namespace Pretorianie.Tytan.Core.NativeImage
{
    public interface IContentDescriptor : IDescriptor
    {
        string Value
        { get; }

        object Source
        { get; }

        void Render();
    }
}
