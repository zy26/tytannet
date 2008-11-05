using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.BaseGenerators
{
    [ComImport, InterfaceType((short)1), Guid(VsGuid.SVsGeneratorProgress)]
    public interface IVsGeneratorProgress
    {
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int GeneratorError([In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.BOOL")] int fWarning, [In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.DWORD")] uint dwLevel, [In, MarshalAs(UnmanagedType.BStr)] string bstrError, [In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.DWORD")] uint dwLine, [In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.DWORD")] uint dwColumn);
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int Progress([In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.ULONG")] uint nComplete, [In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.ULONG")] uint nTotal);
    }
}
