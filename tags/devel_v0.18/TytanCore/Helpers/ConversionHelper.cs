using System.Globalization;
using Pretorianie.Tytan.Core.Interfaces;

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
        public static string ToHex(string number, CodeModelLanguages language)
        {
            if (!string.IsNullOrEmpty(number))
            {
                uint value;

                if (uint.TryParse(number, out value))
                    return ToHex(value, language);

                if (uint.TryParse(number, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))
                    return ToHex(value, language);

                if ((number.StartsWith("0x") || number.StartsWith("0X") || number.StartsWith("&H")) &&
                    uint.TryParse(number.Substring(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))
                    return ToHex(value, language);
            }

            // parsing failed, return string:
            return number;
        }

        /// <summary>
        /// Serialize given number into hex representation.
        /// </summary>
        public static string ToHex(uint value, CodeModelLanguages language)
        {
            switch ( language)
            {
                case CodeModelLanguages.VisualBasic:
                    return "&H" + value.ToString("X4");

                default:
                    return "0x" + value.ToString("X4");
            }
        }

        #endregion
    }
}
