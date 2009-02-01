using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Pretorianie.Tytan.Core.BaseGenerators
{
    /// <summary>
    /// Wrapper class for VisualStudio object implementing IVsSingleFileGenerator interface.
    /// </summary>
    public abstract class BaseCodeGenerator : IVsSingleFileGenerator
    {
        internal const int S_OK = 0;
        internal const int E_FAIL = unchecked((int)0x80004005);
        internal const int E_POINTER = unchecked((int)0x80000005);
        internal const int E_NOINTERFACE = unchecked((int)0x80000004);


        private IVsGeneratorProgress codeGeneratorProgress;
        private string inputFileNamespace = string.Empty;
        private string inputFilePath = string.Empty;

        #region IVsSingleFileGenerator Members

        public int DefaultExtension(out string pbstrDefaultExtension)
        {
            try
            {
                pbstrDefaultExtension = CodeDefaultExtension;
                return S_OK;
            }
            catch (Exception)
            {
                pbstrDefaultExtension = string.Empty;
                return E_FAIL;
            }
        }

        public int Generate(string wszInputFilePath, string bstrInputFileContents,
                            string wszDefaultNamespace, IntPtr[] rgbOutputFileContents,
                            out uint pcbOutputFileContentsSize, IVsGeneratorProgress pGenerateProgress)
        {
            if (bstrInputFileContents == null)
                throw new ArgumentNullException(bstrInputFileContents);

            inputFilePath = wszInputFilePath;
            inputFileNamespace = wszDefaultNamespace;
            codeGeneratorProgress = pGenerateProgress;
            byte[] bytes = null;

            try
            {
                bytes = GenerateByteCode(bstrInputFileContents);
            }
            catch
            {
            }

            if (bytes == null)
            {
                // signal that code generation has failed:
                pcbOutputFileContentsSize = 0;
                return E_FAIL;
            }

            // user of IVsSingleFileGenerator expects data returned via CoTaskMemAlloc()...

            int outputLength = bytes.Length;
            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(outputLength);
            Marshal.Copy(bytes, 0, rgbOutputFileContents[0], outputLength);
            pcbOutputFileContentsSize = (uint) outputLength;
            return S_OK;
        }

        #endregion

        #region Properties


        /// <summary>
        /// Namespace for the file
        /// </summary>
        protected string FileNamespace
        {
            get
            {
                return inputFileNamespace;
            }
        }

        /// <summary>
        /// File-path for the input file
        /// </summary>
        protected string InputFilePath
        {
            get
            {
                return inputFilePath;
            }
        }

        /// <summary>
        /// Interface to the VS shell object we use to tell our progress while we are generating
        /// </summary>
        internal IVsGeneratorProgress CodeGeneratorProgress
        {
            get
            {
                return codeGeneratorProgress;
            }
        }

        /// <summary>
        /// Method that will communicate an error via the shell callback mechanism
        /// </summary>
        /// <param name="level">Level or severity</param>
        /// <param name="message">Text displayed to the user</param>
        /// <param name="line">Line number of error</param>
        /// <param name="column">Column number of error</param>
        protected virtual void GeneratorError(uint level, string message, uint line, uint column)
        {
            if (codeGeneratorProgress != null)
                codeGeneratorProgress.GeneratorError(0, level, message, line, column);
        }

        /// <summary>
        /// Method that will communicate a warning via the shell callback mechanism
        /// </summary>
        /// <param name="level">Level or severity</param>
        /// <param name="message">Text displayed to the user</param>
        /// <param name="line">Line number of warning</param>
        /// <param name="column">Column number of warning</param>
        protected virtual void GeneratorWarning(uint level, string message, uint line, uint column)
        {
            if (codeGeneratorProgress != null)
                codeGeneratorProgress.GeneratorError(1, level, message, line, column);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Gets the default extension for this generator
        /// </summary>
        /// <returns>String with the default extension for this generator</returns>
        protected abstract string CodeDefaultExtension
        { get; }

        /// <summary>
        /// The method that does the actual work of generating he output source code.
        /// </summary>
        /// <param name="inputFileContent">File contents as a string</param>
        /// <returns>The generated code file as a byte-array</returns>
        protected abstract string GenerateStringCode(string inputFileContent);

        /// <summary>
        /// The method that does the actual work of generating he output source code.
        /// </summary>
        /// <param name="inputFileContent">File contents as a string</param>
        /// <returns>The generated code file as a byte-array</returns>
        protected virtual byte[] GenerateByteCode(string inputFileContent)
        {
            string result = GenerateStringCode(inputFileContent);

            if(string.IsNullOrEmpty(result))
                return null;

            // return as bytes:
            return Encoding.UTF8.GetBytes(result);
        }

        #endregion

        #region Automatic COM Registration

        private static readonly string[] SupportedVS = new string[] { "8.0", "8.0Exp", "9.0", "9.0Exp" };

        [ComRegisterFunction]
        public static void RegisterClass(Type comType)
        {
            int i = 0;
            CodeGeneratorRegistrationAttribute[] attrs = (CodeGeneratorRegistrationAttribute[])
                                                         comType.GetCustomAttributes(
                                                             typeof (CodeGeneratorRegistrationAttribute), true);

            Console.Write("Registering class: {0}", comType.Name);

            if (attrs != null)
                foreach (CodeGeneratorRegistrationAttribute a in attrs)
                {
                    foreach (string vsVersion in SupportedVS)
                        a.Register(false, vsVersion);
                    if (i == 0)
                        Console.Write(" as generator: {0}", a.GeneratorName);
                    else
                        Console.Write(", {0}", a.GeneratorName);
                    i++;
                }

            Console.WriteLine();
        }

        [ComUnregisterFunction]
        public static void UnregisterClass(Type comType)
        {
            CodeGeneratorRegistrationAttribute[] attrs = (CodeGeneratorRegistrationAttribute[])
                                                         comType.GetCustomAttributes(
                                                             typeof (CodeGeneratorRegistrationAttribute), true);

            if (attrs != null)
                foreach (CodeGeneratorRegistrationAttribute a in attrs)
                {
                    foreach (string vsVersion in SupportedVS)
                        a.Unregister(false, vsVersion);

                    Console.WriteLine("Removed generator: {0}", a.GeneratorName);
                }
        }

        #endregion
    }
}