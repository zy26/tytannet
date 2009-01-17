using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Section description containing info about all resources exposed by given COFF file.
    /// </summary>
    public class ResourceSection : BinarySection
    {
        /// <summary>
        /// Name of this section.
        /// </summary>
        public const string DefaultName = "Resource";
    }
}
