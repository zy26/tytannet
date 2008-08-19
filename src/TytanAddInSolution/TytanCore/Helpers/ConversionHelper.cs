using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class that provides conversion support.
    /// </summary>
    public static class ConversionHelper
    {
        #region Hex Conversions

        /// <summary>
        /// Converts given number into hex string.
        /// </summary>
        public static string ToHex(string number)
        {
            uint value;

            if (!string.IsNullOrEmpty(number))
            {
                if (uint.TryParse(number, out value))
                    return ToHex(value);

                if (uint.TryParse(number, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))
                    return ToHex(value);

                if ((number.StartsWith("0x") || number.StartsWith("0X")) &&
                    uint.TryParse(number.Substring(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))
                    return ToHex(value);
            }

            // parsing failed, return string:
            return number;
        }

        /// <summary>
        /// Serialize given number into hex representation.
        /// </summary>
        public static string ToHex(uint value)
        {
            return "0x" + value.ToString("X4");
        }

        #endregion
    }
}
