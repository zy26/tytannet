namespace Pretorianie.Tytan.Core.NativeImage
{
    public interface IPropertyDescriptor : IDescriptor
    {
        string Value
        { get; set; }

        object Source
        { get; }

        bool CanUpdate
        { get; }
    }
}
