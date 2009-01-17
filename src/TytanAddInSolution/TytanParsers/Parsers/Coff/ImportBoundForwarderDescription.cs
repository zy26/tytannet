using System;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Info about forwarder of import module.
    /// </summary>
    public class ImportBoundForwarderDescription
    {
        public string Name { get; private set; }
        public DateTime BoundDate { get; private set; }

        /// <summary>
        /// Init constructor of ImportBoundForwarderDescription.
        /// </summary>
        public ImportBoundForwarderDescription(string name, DateTime boundDate)
        {
            Name = name;
            BoundDate = boundDate;
        }
    }
}
