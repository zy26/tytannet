using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Pretorianie.Tytan.Utils
{
    public static class TypeHelper
    {
        public static IList<Type> GetTypes(IList<Assembly> sourceAssemblies, IntPtr comObjectInstance)
        {
            IList<Type> types = new List<Type>();

            if (comObjectInstance != IntPtr.Zero && sourceAssemblies != null && sourceAssemblies.Count > 0)
            {
                try
                {
                    foreach (Assembly a in sourceAssemblies)
                    {
                        try
                        {
                            foreach (Type t in a.GetTypes())
                            {
                                Guid typeGuid = t.GUID;

                                // if valid COM-type:
                                if (typeGuid != Guid.Empty)
                                {
                                    // ask for interface implementation:
                                    IntPtr ppv;
                                    int hResult = Marshal.QueryInterface(comObjectInstance, ref typeGuid, out ppv);

                                    // release gathered handle and store the type description:
                                    if (hResult == 0 && ppv != IntPtr.Zero)
                                    {
                                        Marshal.Release(ppv);
                                        types.Add(t);
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
            }

            return types;
        }

        public static IList<Assembly> StartsWith(IList<Assembly> assemblies, params string[] names)
        {
            if(assemblies != null)
            {
                IList<Assembly> result = new List<Assembly>();

                foreach (Assembly a in assemblies)
                {
                    // check all names:
                    foreach (string n in names)
                    {
                        if (a.FullName.StartsWith(n, StringComparison.CurrentCultureIgnoreCase))
                        {
                            result.Add(a);
                            break;
                        }
                    }
                }

                return result;
            }

            return null;
        }
    }
}
