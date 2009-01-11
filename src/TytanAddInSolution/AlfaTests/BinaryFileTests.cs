using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pretorianie.Tytan.Parsers.Coff;
using Pretorianie.Tytan.Parsers.Model;

namespace AlfaTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class BinaryFileTests
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private static void DumpSections(BinaryFile file)
        {
            if (file == null)
            {
                Trace.WriteLine("Invalid BinaryFile provided for dump!");
                return;
            }

            // dump sections:
            foreach (BinarySection s in file.Sections)
                Trace.WriteLine(string.Format("Section: \"{0}\" - address: 0x{1:X8} - size: {2} byte(s) (0x{2:X4})", s.Name,
                                              s.VirtualAddress, s.VirtualSize));
        }

        private static void DumpExportSection(BinaryFile file)
        {
            if (file == null)
                return;

            // get export section:
            ExportFunctionSection export = file["Export"] as ExportFunctionSection;

            if (export != null)
            {
                Trace.WriteLine("Exported functions:");
                foreach (ExportFunctionDescription f in export.Functions)
                    Trace.WriteLine(string.Format("Function found: \"{0}\" (@{2}) at address: 0x{1:X8}{3}", f.Name,
                                                  f.VirtualAddress, f.Ordinal,
                                                  f.IsForwarded ? " (forwarded to: " + f.ForwardedName + ")" : string.Empty));
            }
        }

        private void ValidateSections(BinaryFile file)
        {
            if (file == null || file.Sections == null)
                return;

            // each section must have a name & valid address:
            foreach (BinarySection s in file.Sections)
            {
                Assert.IsNotNull(s.Name);
                Assert.IsNotNull(s.VirtualAddress);
                Assert.IsTrue(s.VirtualSize > 0, "Size can't be equal to 0!");
            }
        }

        [TestMethod]
        [DeploymentItem("DataSources/NanoStorm.exe")]
        [DeploymentItem("DataSources/AdvSection.obj")]
        [DeploymentItem("DataSources/NanoHooks.dll")]
        public void LoadDLL_NanoHooks()
        {
            BinaryFile file = new WindowsPE();

            file.Load("NanoHooks.dll", null);
            //file.Load("NanoStorm.exe", null);
            //file.Load("AdvSection.obj", null);
            //file.Load(@"E:\a.txt", null);

            // dump sections:
            DumpSections(file);
            DumpExportSection(file);

            // validate if something has been read:
            Assert.IsNotNull(file.Sections);
            ValidateSections(file);
        }

        [TestMethod]
        public void LoadDLL_Kernell32()
        {
            BinaryFile file = new WindowsPE();

            file.Load(@"C:\windows\system32\KERNEL32.DLL", null);

            // dump all data:
            DumpSections(file);
            DumpExportSection(file);

            // validate:
            Assert.IsNotNull(file.Sections);
            ValidateSections(file);
        }
    }
}
