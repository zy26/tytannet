using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.BaseGenerators
{
    [ComImport, InterfaceType((short)1), Guid(VsGuid.SObjectWithSite), ComConversionLoss]
    public interface IObjectWithSite
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] object pUnkSite);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetSite([In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.REFIID")] ref Guid riid, out IntPtr ppvSite);
    }
}
