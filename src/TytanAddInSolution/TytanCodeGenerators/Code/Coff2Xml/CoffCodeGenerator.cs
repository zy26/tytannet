using System;
using System.Runtime.InteropServices;
using System.Xml;
using Pretorianie.Tytan.Core.BaseGenerators;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.Interfaces;
using Pretorianie.Tytan.Parsers.Coff;
using Pretorianie.Tytan.Parsers.Model;

namespace Pretorianie.Tytan.Code.Coff2Xml
{
    /// <summary>
    /// This is the generator class. 
    /// When setting the 'Custom Tool' property of a C#, VB, or J# project item to "Coff2XmlGenerator", 
    /// the GenerateCode function will get called and will return the contents of the generated file 
    /// to the project system
    /// </summary>
    [ComVisible(true)]
    [Guid("ED212885-A631-4844-AC4F-9A35CEDBD757")]
    [CodeGeneratorRegistration(typeof(CoffCodeGenerator), CoffCodeGenerator.GeneratorName,
        Description = "Generates XML file based on specified input COFF file (exe, dll, lib, ocx).",
        GeneratesCSharpSourceCode = true, GeneratesVBasicSouceCode = true, GeneratesDesignTimeSource = true)]
    public class CoffCodeGenerator : BaseCodeGeneratorWithSite
    {
        #region Xml Node Constants

        private const string GeneratorName = "Coff2XmlGenerator";
        private const string CurrentVersion = "1.0";

        private const string NodeMainName = "coff2xml";

        private const string NodeInfo = "info";
        private const string NodeInfoVersion = "version";
        private const string NodeInfoGenerator = "generator";
        private const string NodeInfoProcessedFile = "file";
        private const string NodeInfoGenerationDate = "date";

        private const string NodeExports = "exports";
        private const string NodeExportsFunctionForwardedName = "forwarded";

        private const string NodeImports = "imports";
        private const string NodeImportsModule = "module";
        private const string NodeImportsModuleName = "name";

        private const string NodeForwarder = "forwarder";
        private const string NodeForwarderName = "name";
        private const string NodeForwarderBoundDate = "date";

        private const string NodeFunction = "function";
        private const string NodeFunctionName = "name";
        private const string NodeFunctionOrdinal = "ordinal";
        private const string NodeFunctionAddress = "address";

        #endregion

        /// <summary>
        /// Define the default file extension.
        /// </summary>
        protected override string CodeDefaultExtension
        {
            get
            {
                return ".xml";
            }
        }

        /// <summary>
        /// Generate corresponding XML code.
        /// </summary>
        protected override string GenerateStringCode(string inputFileContent)
        {
            try
            {
                BinaryFile file = new WindowsPortableExecutable();
                WindowsPortableExecutableLoadArgs args = new WindowsPortableExecutableLoadArgs(false);
                XmlDocumentWrapper xml;
                XmlNode mainNode;
                bool[] externalParams;

                args.LoadImports = true;
                args.LoadExports = true;

                // load the file:
                file.Load(InputFilePath, args);

                // check integrity:
                if (file.Count < 2)
                    return string.Format(CoffComments.InvalidContent, InputFilePath);

                // create output XML document:
                xml = new XmlDocumentWrapper();
                xml.DeclareDocument();
                xml.Append(mainNode = DefineMainElement(xml));

                // interprete extenal parameters:
                InterpreteArguments((string.IsNullOrEmpty(FileNamespace) ? null : FileNamespace.Split(';')),
                                    out externalParams);

                // serialize proper sections:
                if (externalParams == null || externalParams.Length == 0 || externalParams[0])
                    AppendSection(xml, mainNode, file[ExportFunctionSection.DefaultName] as ExportFunctionSection);

                if (externalParams == null || externalParams.Length <= 1 || externalParams[1])
                    AppendSection(xml, mainNode, file[ImportFunctionSection.DefaultName] as ImportFunctionSection);

                // and return data as a string:
                return XmlHelper.ToString(xml, true);
            }
            catch (Exception ex)
            {
                return string.Format(CoffComments.InvalidOperation, inputFileContent, ex.Message);
            }
        }

        private void InterpreteArguments(string[] args, out bool[] externalParams)
        {
            externalParams = null;

            if (args != null && args.Length > 0)
            {
                int i = 0;

                externalParams = new bool[args.Length];

                // check if given parameter can be somehow interprete as bool value:
                foreach (string a in args)
                    externalParams[i++] = string.IsNullOrEmpty(a) || a == "1" || bool.Parse(a);
            }
        }

        #region Serialization

        private XmlNode SerializeBaseFunction(XmlDocumentWrapper xml, BaseFunctionDescription desc, XmlNode optional)
        {
            XmlNode e = xml.CreateElement(NodeFunction);
            XmlNode n = xml.CreateElementWithText(NodeFunctionName, desc.Name);
            XmlNode o = xml.CreateElementWithText(NodeFunctionOrdinal, desc.Ordinal.ToString());
            XmlNode a = xml.CreateElementWithText(NodeFunctionAddress,
                                                  "0x" + desc.VirtualAddress.ToString("X8"));

            // append all info about given function:
            if (n != null)
                e.AppendChild(n);
            if (optional != null)
                e.AppendChild(optional);
            if (o != null)
                e.AppendChild(o);
            if (a != null)
                e.AppendChild(a);

            return e;
        }

        private XmlNode SerializeImportForwarder(XmlDocumentWrapper xml, ImportBoundForwarderDescription s)
        {
            XmlNode f = xml.CreateElement(NodeForwarder);

            if (f != null)
            {
                XmlNode n = xml.CreateElementWithText(NodeForwarderName, s.Name);
                XmlNode d = xml.CreateElementWithText(NodeForwarderBoundDate, s.BoundDate.ToString());

                if (n != null)
                    f.AppendChild(n);
                if (d != null)
                    f.AppendChild(d);
            }

            return f;
        }

        private void AppendSection(XmlDocumentWrapper xml, XmlNode main, ExportFunctionSection s)
        {
            if (main == null || s == null || s.Count <= 0)
                return;

            XmlNode exports = xml.CreateElement(NodeExports);

            if (exports == null)
                return;

            foreach (ExportFunctionDescription desc in s.Functions)
            {
                XmlNode f = (desc.IsForwarded
                                 ? xml.CreateElementWithText(NodeExportsFunctionForwardedName,
                                                             desc.ForwardedName)
                                 : null);
                XmlNode e = SerializeBaseFunction(xml, desc, f);

                // append function info to exports:
                if (e != null)
                    exports.AppendChild(e);
            }

            // store:
            main.AppendChild(exports);
        }

        private XmlNode SerializeImportModule(XmlDocumentWrapper xml, ImportFunctionModule s)
        {
            XmlNode m = xml.CreateElement(NodeImportsModule);

            if (m != null)
            {
                XmlNode n = xml.CreateElementWithText(NodeImportsModuleName, s.Name);

                if (n != null)
                    m.AppendChild(n);

                // serialize imported forwarders:
                foreach (ImportBoundForwarderDescription desc in s.Forwarders)
                {
                    XmlNode f = SerializeImportForwarder(xml, desc);

                    if (f != null)
                        m.AppendChild(f);
                }

                // serialize imported functions:
                foreach (ImportFunctionDescription desc in s.Functions)
                {
                    XmlNode f = SerializeBaseFunction(xml, desc, null);

                    if (f != null)
                        m.AppendChild(f);
                }
            }

            return m;
        }

        private void AppendSection(XmlDocumentWrapper xml, XmlNode main, ImportFunctionSection s)
        {
            if (main == null || s == null || s.Count <= 0)
                return;

            XmlNode imports = xml.CreateElement(NodeImports);

            if (imports == null)
                return;

            // serialize all the import sections:
            foreach (ImportFunctionModule desc in s.Modules)
            {
                XmlNode m = SerializeImportModule(xml, desc);

                if (m != null)
                    imports.AppendChild(m);
            }

            main.AppendChild(imports);
        }

        private XmlNode DefineMainElement(IXmlDocument xml)
        {
            XmlNode n = xml.CreateElement(NodeMainName);
            XmlNode i = xml.CreateElement(NodeInfo);

            if (n == null || i == null)
                return null;

            XmlNode v = xml.CreateElementWithText(NodeInfoVersion, CurrentVersion);
            XmlNode g = xml.CreateElementWithText(NodeInfoGenerator, GeneratorName);
            XmlNode f = xml.CreateElementWithText(NodeInfoProcessedFile, InputFilePath);
            XmlNode d = xml.CreateElementWithText(NodeInfoGenerationDate,
                                                  DateTime.Now.ToString());

            if (v != null)
                i.AppendChild(v);
            if (g != null)
                i.AppendChild(g);
            if (f != null)
                i.AppendChild(f);
            if (d != null)
                i.AppendChild(d);

            n.AppendChild(i);
            return n;
        }

        #endregion
    }
}
