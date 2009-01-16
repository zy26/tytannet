using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Load argument arguments for Windows COFF format.
    /// </summary>
    public class WindowsPortableExecutableLoadArgs : BinaryLoadArgs
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public WindowsPortableExecutableLoadArgs()
            : base(true, true)
        {
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public WindowsPortableExecutableLoadArgs(bool loadAll)
            : base(true, true)
        {
            LoadImports = loadAll;
            LoadDelayedImports = loadAll;
            LoadExports = loadAll;
            LoadResources = loadAll;
            LoadCliInfo = loadAll;
            LoadBaseRelocations = loadAll;
        }

        /// <summary>
        /// Load Import section.
        /// </summary>
        public bool LoadImports { get; set; }
        /// <summary>
        /// Load Delayed Import section.
        /// </summary>
        public bool LoadDelayedImports { get; set; }
        /// <summary>
        /// Load Export section.
        /// </summary>
        public bool LoadExports { get; set; }
        /// <summary>
        /// Load resources.
        /// </summary>
        public bool LoadResources { get; set; }
        /// <summary>
        /// Load .NET header info.
        /// </summary>
        public bool LoadCliInfo { get; set; }
        /// <summary>
        /// Load relocations.
        /// </summary>
        public bool LoadBaseRelocations { get; set; }
    }
}
