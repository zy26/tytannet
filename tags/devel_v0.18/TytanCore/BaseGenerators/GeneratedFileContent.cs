using System.Text;
using EnvDTE;

namespace Pretorianie.Tytan.Core.BaseGenerators
{
    /// <summary>
    /// Description of automatically generated file content.
    /// </summary>
    public class GeneratedFileContent
    {
        private readonly string fileName;
        private readonly byte[] data;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public GeneratedFileContent(string fileName, byte[] content)
        {
            this.fileName = fileName;
            data = content;
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public GeneratedFileContent(string fileName, string content)
        {
            this.fileName = fileName;
            data = Encoding.UTF8.GetBytes(content);
        }

        /// <summary>
        /// Sets additional parameters for new project item added to the solution.
        /// </summary>
        public virtual void SetNewItemProperties (ProjectItem newItem)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the file name of automatically generated item.
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        /// <summary>
        /// Gets the binary content of the file.
        /// </summary>
        public byte[] BinaryData
        {
            get
            {
                return data;
            }
        }

        #endregion

    }
}
