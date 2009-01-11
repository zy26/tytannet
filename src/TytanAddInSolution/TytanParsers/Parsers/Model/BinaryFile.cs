using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Pretorianie.Tytan.Core.Mapping;

namespace Pretorianie.Tytan.Parsers.Model
{
    /// <summary>
    /// Base class describing content of any binary file.
    /// </summary>
    public abstract class BinaryFile
    {
        /// <summary>
        /// Event fired each time new section has been correctly parsed and added.
        /// </summary>
        public event BinarySectionChangedEventHandler SectionAdded;

        private string fullPath;
        private string name;
        private IList<BinarySection> sections;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BinaryFile()
        {
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        protected BinaryFile(string path, string name)
        {
            fullPath = path;
            this.name = name;
        }

        #region Properties

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the path of the file.
        /// </summary>
        public string FullPath
        {
            get { return fullPath; }
        }

        /// <summary>
        /// Gets the list of sections that compose the binary file.
        /// </summary>
        public IEnumerable<BinarySection> Sections
        {
            get { return sections; }
        }

        /// <summary>
        /// Gets the section with specifed name.
        /// </summary>
        public BinarySection this[string section]
        {
            get
            {
                if(sections == null)
                    return null;

                foreach(BinarySection b in sections)
                    if(b.Name == section)
                        return b;

                return null;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Adds new section.
        /// </summary>
        protected void Add(BinarySection s)
        {
            if(sections == null)
                sections = new List<BinarySection>();
            sections.Add(s);

            // and notify listeners:
            if (SectionAdded != null)
                SectionAdded(this, s);
        }

        /// <summary>
        /// Reads given structure from specified source.
        /// </summary>
        protected S Read<T, S>(UnmanagedDataReader source)
            where T : struct
            where S : BinarySection, IBinaryConverter<T>, new()
        {
            T headerNativeType = new T();

            // load native data:
            if (source.Read(ref headerNativeType))
            {
                S section = new S();
                if (section.Convert(ref headerNativeType, source.LastReadStartOffset, source.LastReadSize))
                {
                    Add(section);
                    return section;
                }
            }

            // return failure in reading:
            return null;
        }

        /// <summary>
        /// Reads given structure and appends info to given section.
        /// </summary>
        protected static bool Append<T, S, Z>(UnmanagedDataReader source, S section, Z arg)
            where T : struct
            where S : class, IBinaryAppender<T, Z>
        {
            T headerElement = new T();

            // load native type:
            if (section != null && source.Read(ref headerElement))
                return section.Attach(ref headerElement, source.LastReadSize, arg);

            // return failure:
            return false;
        }

        #endregion

        /// <summary>
        /// Load section descriptions from given file.
        /// </summary>
        public void Load(string fileName, BinaryLoadArgs e)
        {
            fullPath = Path.GetFullPath(fileName);
            name = Path.GetFileName(fileName);

            if (File.Exists(fullPath))
            {
                if (e == null || e.UseMapping)
                {
                    // create readers:
                    UnmanagedDataReader s;
                    FileSharedMemory fs = null;

                    try
                    {
                        fs = new FileSharedMemory(fullPath,
                                                  (e == null || e.IsReadOnlyMode
                                                       ? SharedMemory.AccessTypes.ReadOnly
                                                       : SharedMemory.AccessTypes.ReadWrite));
                        s = new UnmanagedDataReader(fs.Address, fs.Size);

                        // and parse the file:
                        Load(s, e);
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }
                }
                else
                {
                    // read brutally the whole file into memory and parse:
                    byte[] x = File.ReadAllBytes(fullPath);

                    // check if not empty:
                    if (x.Length > 0)
                    {
                        IntPtr p = Marshal.AllocHGlobal(x.Length);

                        if (p != IntPtr.Zero)
                        {
                            Marshal.Copy(x, 0, p, x.Length);
                            Load(new UnmanagedDataReader(p, (uint) x.Length), e);
                            Marshal.FreeHGlobal(p);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads proper binary sections based on given source.
        /// </summary>
        protected abstract void Load(UnmanagedDataReader s, BinaryLoadArgs e);
    }
}