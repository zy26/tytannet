using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Pretorianie.Tytan.Utils;

namespace Pretorianie.Tytan.ObjectSources
{
    /// <summary>
    /// Class that coordinates data transfer.
    /// </summary>
    public class SystemComObjectSource : VisualizerObjectSource
    {
        /// <summary>
        /// Serialize data from the debugee process into the debugger.
        /// </summary>
        public override void GetData(object target, Stream outgoingData)
        {
            IntPtr comObject = (Marshal.IsComObject(target) ? Marshal.GetIUnknownForObject(target) : IntPtr.Zero);
            IList<Assembly> assemblies = TypeHelper.StartsWith(AppDomain.CurrentDomain.GetAssemblies(), "Microsoft", "System", "Pretorianie", "Tytan");
            IList <Type> types = TypeHelper.GetTypes(assemblies, comObject);

            // serialize:
            SerializationHelper.WriteAsBinary(outgoingData, types);
        }
    }
}
