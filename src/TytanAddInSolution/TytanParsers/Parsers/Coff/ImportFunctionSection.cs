using System.Collections.Generic;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Parsers.Coff
{
    /// <summary>
    /// Section describing imported functions of given COFF file.
    /// </summary>
    public class ImportFunctionSection : BinarySection
    {
        private IList<ImportFunctionModule> modules = new List<ImportFunctionModule>();

        /// <summary>
        /// Name of this section.
        /// </summary>
        public const string DefaultName = "Import";

        #region Properties

        public IList<ImportFunctionModule> Modules
        {
            get { return modules; }
        }

        public int Count
        {
            get { return modules.Count; }
        }

        #endregion

        /// <summary>
        /// Gets the imported module with given name.
        /// </summary>
        public ImportFunctionModule this[string name]
        {
            get
            {
                foreach (ImportFunctionModule m in modules)
                    if (m.Name == name)
                        return m;

                return null;
            }
        }

        /// <summary>
        /// Adds new import module.
        /// </summary>
        public void Add(ImportFunctionModule t)
        {
            modules.Add(t);
        }
    }
}
