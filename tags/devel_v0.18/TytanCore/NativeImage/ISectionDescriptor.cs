using System.Collections.Generic;

namespace Pretorianie.Tytan.Core.NativeImage
{
    public interface ISectionDescriptor : IDescriptor
    {
        StructureDescriptorTypes Type
        { get; }

        IList<IPropertyDescriptor> Properties
        { get; }

        IList<IFunctionDescriptor> Functions
        { get; }
    }
}
