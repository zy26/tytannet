using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Pretorianie.Tytan.Core.BaseGenerators;

namespace Pretorianie.Tytan.Code.Header2Cs
{
    /// <summary>
    /// This is the generator class.
    /// When setting the 'Custom Tool' property of a C/C++ header file to "CsGenerator", 
    /// the GenerateCode function will get called and will return the contents of the generated file 
    /// to the project system
    /// </summary>
    [ComVisible(true)]
    [Guid("E73FDA83-E6BE-4589-9B2C-EBD2BFC77B03")]
    [CodeGeneratorRegistration(typeof(Header2CsCodeGenerator), "CsGenerator",
        Description = "Generates .NET source code from given C/C++ header file.",
        GeneratesCSharpSourceCode = true, GeneratesDesignTimeSource = true)]
    public class Header2CsCodeGenerator : BaseMultiCodeGeneratorWithSite
    {
        protected override IList<GeneratedFileContent> GenerateContents(string inputFileContent)
        {
            List<GeneratedFileContent> result = new List<GeneratedFileContent>();

            result.Add(new GeneratedFileContent("a.cs", "1a"));
            result.Add(new GeneratedFileContent("b.cs", "2b"));
            result.Add(new GeneratedFileContent("c.cs", "3a"));
            result.Add(new GeneratedFileContent("d.cs", "4f"));
            result.Add(new GeneratedFileContent("e.cs", "5"));

            return result;
        }

        protected override string GenerateSummary(string inputFileContent)
        {
            return "created 5 files " + Path.GetDirectoryName(GetProjectItem().get_FileNames(0));
        }
    }
}
