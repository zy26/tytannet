using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class that wraps export of given registry node as text content.
    /// </summary>
    public static class RegistryHelper
    {
        /// <summary>
        /// Exports given registry node into text compatible with regedit format.
        /// </summary>
        public static string ExportRegistry(RegistryKey key)
        {
            StringBuilder regResult = new StringBuilder();

            regResult.AppendFormat("[{0}]", key.Name).AppendLine();
            try
            {
                string[] values = key.GetValueNames();

                foreach (string v in values)
                {
                    try
                    {
                        RegistryValueKind kind = key.GetValueKind(v);
                        object value = key.GetValue(v);

                        // export name and value inside 'one line':
                        regResult.AppendFormat("\"{0}\"=", v);
                        ExportValue(regResult, kind, value);
                        regResult.AppendLine();
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
                Trace.Write(ex.StackTrace);
            }

            try
            {
                string[] keys = key.GetSubKeyNames();

                foreach (string k in keys)
                {
                    RegistryKey subKey = null;

                    try
                    {
                        // export recursively all the children:
                        subKey = key.OpenSubKey(k, false);
                        if (subKey != null)
                            regResult.Append(ExportRegistry(subKey));
                    }
                    catch (Exception ex)
                    {
                        Trace.Write(ex.Message);
                        Trace.Write(ex.StackTrace);
                    }
                    finally
                    {
                        if (subKey != null)
                            subKey.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
            }

            return regResult.ToString();
        }

        /// <summary>
        /// Write array of bytes.
        /// </summary>
        private static void ToHex(StringBuilder output, byte[] value)
        {
            if (value != null && value.Length > 0)
            {
                // append all the binary values separated by comma:
                output.AppendFormat("{0:X2}", value[0]);

                for (int i = 1; i < value.Length; i++)
                    if (i % 21 == 0)
                        output.AppendFormat(",\\\r\n  {0:X2}", value[i]);
                    else
                        output.AppendFormat(",{0:X2}", value[i]);
            }
        }

        /// <summary>
        /// Write long value as bytes.
        /// </summary>
        private static void ToHex(StringBuilder output, long value)
        {
            output.AppendFormat("{0:X2}", value & 0xFF);

            for (int i = 1; i < 8; i++)
            {
                value >>= 8;
                output.AppendFormat(",{0:X2}", value & 0xFF);
            }
        }

        /// <summary>
        /// Serialize UTF-16 string as array of bytes.
        /// </summary>
        private static byte[] ToBinArray(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                // each string must finish with '\0' character:
                byte[] result = new byte[value.Length * 2 + 2];
                Encoding.Unicode.GetBytes(value, 0, value.Length, result, 0);

                return result;
            }

            return null;
        }

        /// <summary>
        /// Serialize array of UTF-16 strings as array of bytes.
        /// </summary>
        private static byte[] ConvertToBin(IEnumerable<string> value)
        {
            if (value == null)
                return null;

            byte[] result;
            int size = 0;
            int index = 0;

            // calculate size - each char is 2-bytes long, each string finishes with '\0' and...:
            foreach (string s in value)
                size += (s != null ? 2 * s.Length : 0) + 2;
            // at the end there is an empty string with only '\0'...
            size += 2;

            // allocate output buffer:
            result = new byte[size];
            foreach (string s in value)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    Encoding.Unicode.GetBytes(s, 0, s.Length, result, index);
                    index += 2 * s.Length;
                }

                index += 2;
            }

            return result;
        }

        private static string ToSecureString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        private static void ExportValue(StringBuilder output, RegistryValueKind kind, object value)
        {
            switch (kind)
            {
                case RegistryValueKind.String:
                    output.AppendFormat("\"{0}\"", ToSecureString(value as string));
                    break;
                case RegistryValueKind.DWord:
                    output.AppendFormat("dword:{0:X}", value);
                    break;
                case RegistryValueKind.QWord:
                    output.Append("hex(b)");
                    ToHex(output, (long)value);
                    break;
                case RegistryValueKind.Binary:
                    output.Append("hex:");
                    ToHex(output, value as byte[]);
                    break;
                case RegistryValueKind.ExpandString:
                    output.AppendFormat("hex(2):");
                    ToHex(output, ToBinArray(value as string));
                    break;
                case RegistryValueKind.MultiString:
                    output.AppendFormat("hex(7):");
                    ToHex(output, ConvertToBin(value as string[]));
                    break;
            }
        }
    }
}
