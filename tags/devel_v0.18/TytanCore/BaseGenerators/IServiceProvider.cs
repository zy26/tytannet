using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.BaseGenerators
{
    [ComImport, InterfaceType((short)1), Guid(VsGuid.SServiceProvider)]
    public interface IServiceProvider
    {
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int QueryService([In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.REFGUID")] ref Guid guidService, [In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.REFIID")] ref Guid riid, out IntPtr ppvObject);
    }
}
