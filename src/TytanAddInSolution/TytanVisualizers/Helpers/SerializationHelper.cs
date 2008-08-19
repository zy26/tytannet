using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pretorianie.Tytan.Utils
{
    /// <summary>
    /// Helper class that serializes objects.
    /// </summary>
    public static class SerializationHelper
    {
        /// <summary>
        /// Writes binary result of the serialization into specified output.
        /// </summary>
        public static void WriteAsBinary(Stream output, object input)
        {
            if (input != null)
            {
                IFormatter formatter = new BinaryFormatter();

                // serialize data:
                formatter.Serialize(output, input);
            }
        }
    }
}
