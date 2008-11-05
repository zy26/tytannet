using System;
using System.IO;
using System.Text;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class for file operations.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Converts content of given file into string containing hex values of read bytes.
        /// This method will throw any kind of exceptions in case of file access violations or data conversions.
        /// </summary>
        public static string Import(string fileName)
        {
            // check if given file exists, so there is something to read...
            if (!File.Exists(fileName))
                return null;

            // convert the content of a file into hex-string:
            StringBuilder result = new StringBuilder();
            FileStream file = File.OpenRead(fileName);
            int currentByte;

            while (file.CanRead)
            {
                currentByte = file.ReadByte();
                if (currentByte == -1)
                    break;

                result.Append(currentByte.ToString("X2"));
            }

            // and get the whole generated text:
            return result.ToString();
        }

        /// <summary>
        /// Converts hex representation of a file into binary data.
        /// </summary>
        public static void Export(string fileName, string hexContent)
        {
            FileStream file = File.Create(fileName, 2048);

            try
            {
                if (!string.IsNullOrEmpty(hexContent))
                {
                    hexContent = hexContent.Trim().ToUpper();

                    int i = 0;
                    int length = hexContent.Length;

                    // convert data into byte and write it:
                    while (i + 1 < length)
                    {
                        int value;
                        char c1 = hexContent[i++];
                        char c2 = hexContent[i++];

                        // interprete first character:
                        if (c1 >= '0' && c1 <= '9')
                            value = (c1 - '0') << 4;
                        else if (c1 >= 'A' && c1 <= 'F')
                            value = (10 + (c1 - 'A')) << 4;
                        else
                            throw new ArgumentOutOfRangeException("hexContent",
                                                                  "Invalid character occurred, file is partially valid");

                        // interprete second character:
                        if (c2 >= '0' && c2 <= '9')
                            value += c2 - '0';
                        else if (c2 >= 'A' && c2 <= 'F')
                            value += 10 + (c2 - 'A');
                        else
                            throw new ArgumentOutOfRangeException("hexContent",
                                                                  "Invalid character occurred, file is truncated");

                        // write the value into the file:
                        file.WriteByte((byte) value);
                    }
                }
            }
            finally
            {
                file.Close();
            }
        }
    }
}
