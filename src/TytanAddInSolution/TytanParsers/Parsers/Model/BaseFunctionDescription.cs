﻿namespace Pretorianie.Tytan.Parsers.Model
{
    /// <summary>
    /// Base class for all types of functions imported/exported by binary files.
    /// </summary>
    public class BaseFunctionDescription
    {
        private readonly string name;
        private readonly uint ordinal;
        private readonly ulong address;

        /// <summary>
        /// Init constructor of BaseFunctionDescription.
        /// </summary>
        public BaseFunctionDescription(string name, uint ordinal, ulong address)
        {
            this.name = name;
            this.ordinal = ordinal;
            this.address = address;
        }

        #region Properties

        /// <summary>
        /// Gets the value of Name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the value of Ordinal.
        /// </summary>
        public uint Ordinal
        {
            get
            {
                return ordinal;
            }
        }

        /// <summary>
        /// Gets the value of Address.
        /// </summary>
        public ulong VirtualAddress
        {
            get
            {
                return address;
            }
        }

        #endregion
    }
}
