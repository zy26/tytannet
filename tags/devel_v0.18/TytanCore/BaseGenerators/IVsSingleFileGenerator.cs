using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Core.BaseGenerators
{
    [ComImport, Guid(VsGuid.SVsSingleFileGenerator), InterfaceType((short)1), ComConversionLoss]
    public interface IVsSingleFileGenerator
    {
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int DefaultExtension([MarshalAs(UnmanagedType.BStr)] out string pbstrDefaultExtension);
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int Generate([In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.LPCOLESTR"), MarshalAs(UnmanagedType.LPWStr)] string wszInputFilePath, [In, MarshalAs(UnmanagedType.BStr)] string bstrInputFileContents, [In, ComAliasName("Microsoft.VisualStudio.OLE.Interop.LPCOLESTR"), MarshalAs(UnmanagedType.LPWStr)] string wszDefaultNamespace, [Out, ComAliasName("Microsoft.VisualStudio.TextManager.Interop.BYTE"), MarshalAs(UnmanagedType.LPArray)] IntPtr[] rgbOutputFileContents, [ComAliasName("Microsoft.VisualStudio.OLE.Interop.ULONG")] out uint pcbOutput, [In, MarshalAs(UnmanagedType.Interface)] IVsGeneratorProgress pGenerateProgress);
    }
}
